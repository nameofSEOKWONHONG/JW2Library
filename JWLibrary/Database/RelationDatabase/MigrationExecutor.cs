using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using eXtensionSharp;
using Microsoft.Scripting.Utils;
using MySql.Data.MySqlClient;
using Npgsql;

namespace JWLibrary.Database
{
    public interface IMigration
    {
        bool IsExistsTable(IDbConnection connection);
        void CreateTable(IDbConnection connection);
        void CreateTempTable(IDbConnection connection);
        void AfterProcess(IDbConnection connection);
    }

    public abstract class JMigration : IMigration
    {
        public abstract bool IsExistsTable(IDbConnection connection);
        public abstract void CreateTable(IDbConnection connection);
        public abstract void CreateTempTable(IDbConnection connection);
        public abstract void AfterProcess(IDbConnection connection);
    }

    public class MigrationExecutor
    {
        private readonly IEnumerable<string> _dllPaths;

        public MigrationExecutor(string[] dllPaths)
        {
            if (dllPaths.xIsEmpty())
                throw new NullReferenceException("dll path is empty.");

            _dllPaths = dllPaths;
        }

        public void Execute()
        {
            var list = new XList<IMigration>();
            _dllPaths.xForEach(path =>
            {
                var instances = path.CreateInstance<IMigration>(new[] {"Migration"}, new[] {"MigrationExecutor"});
                list.AddRange(instances);
            });

            list.xForEach(instance =>
            {
                //table new = table create
                //table exists = table backup > create new table > data copy
                IDbConnection connection = null;
                if (instance.GetType().Name.Contains("MS"))
                    connection = new SqlConnection(DbConnectionProvider.Instance.MSSQL);
                else if (instance.GetType().Name.Contains("MY"))
                    connection = new MySqlConnection(DbConnectionProvider.Instance.MYSQL);
                else if (instance.GetType().Name.Contains("NPG"))
                    connection = new NpgsqlConnection(DbConnectionProvider.Instance.POSTGRESQL);
                else if (instance.GetType().Name.Contains("Sqlite"))

                    if (instance.xIsNull())
                        throw new Exception("not found database provider");

                var trans = connection.BeginTransaction();

                try
                {
                    if (instance.IsExistsTable(connection))
                    {
                        instance.CreateTempTable(connection);
                        instance.AfterProcess(connection);
                    }
                    else
                    {
                        instance.CreateTable(connection);
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            });
        }
    }
}
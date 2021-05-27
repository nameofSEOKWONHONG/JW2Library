using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;

namespace JWLibrary.Database {
    /// <summary>
    ///     create database connection Resolver
    /// </summary>
    public class JDatabaseResolver {
        public static JDbClientExecutor Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            IDbConnection connection = null;
            if (typeof(TDatabase) == typeof(Microsoft.Data.SqlClient.SqlConnection)) {
                
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.MSSQL);
            }
            else if (typeof(TDatabase) == typeof(MySqlConnection)) {
                
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.MYSQL);
            }
            else if (typeof(TDatabase) == typeof(NpgsqlConnection)) {
                
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.POSTGRESQL);
            }
            else {
                throw new NotImplementedException();    
            }

            var executor = new JDbClientExecutor(connection);
            return executor;
        }

        public static JDbClientExecutor Resolve(ENUM_DATABASE_TYPE type) {
            IDbConnection connection = null;
            if (type == ENUM_DATABASE_TYPE.MSSQL) {
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.MSSQL);
            }
            else if (type == ENUM_DATABASE_TYPE.MYSQL) {
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.MYSQL);
            }
            else if (type == ENUM_DATABASE_TYPE.POSTGRESQL) {
                connection = JDatabaseInfo.Instance.GetConnection(ENUM_DATABASE_TYPE.POSTGRESQL);
            }
            else {
                throw new NotImplementedException();
            }

            var executor = new JDbClientExecutor(connection);
            return executor;
        }
    }
}
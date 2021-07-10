using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using RepoDb;

namespace JWLibrary.Database
{
    /// <summary>
    ///     Database Information Master
    /// </summary>
    internal class JDatabaseInfo
    {
        #region [1st class variable]

        private readonly Dictionary<string, Func<string, IDbConnection>> _connectionMaps = new()
        {
            {
                ENUM_DATABASE_TYPE.MSSQL.ToString(), connectionString =>
                {
                    //no more use System.Data.SqlClient.SqlConnection
                    //replace Microsoft.Data.SqlClient.SqlConnection
                    SqlServerBootstrap.Initialize();
                    return new SqlConnection(DbConnectionProvider.Instance.MSSQL);
                }
            },
            {
                ENUM_DATABASE_TYPE.MYSQL.ToString(), connectionString =>
                {
                    MySqlBootstrap.Initialize();
                    return new MySqlConnection(DbConnectionProvider.Instance.MYSQL);
                }
            },
            {
                ENUM_DATABASE_TYPE.POSTGRESQL.ToString(), connectionString =>
                {
                    PostgreSqlBootstrap.Initialize();
                    return new NpgsqlConnection(DbConnectionProvider.Instance.POSTGRESQL);
                }
            }
            // {
            //     "ORACLE", connectionString => {
            //         return new OracleConnection(connectionString);
            //     }
            // },
        };

        #endregion

        #region [ctor]

        #endregion

        public Dictionary<string, IDbConnection> Connections { get; } =
            new();

        public IDbConnection GetConnection(ENUM_DATABASE_TYPE dbType)
        {
            return _connectionMaps[dbType.ToString()].Invoke(dbType.ToString());
        }

        #region [lazy instance]

        private static readonly Lazy<JDatabaseInfo> _instance =
            new(() => { return new JDatabaseInfo(); });

        public static JDatabaseInfo Instance => _instance.Value;

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using eXtensionSharp;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Npgsql;
using RepoDb;

namespace JWLibrary.Database {
    /// <summary>
    ///     Database Information Master
    /// </summary>
    internal class JDatabaseInfo {
        #region [lazy instance]
        private static readonly Lazy<JDatabaseInfo> _instance = 
            new(() => { return new JDatabaseInfo(); });

        public static JDatabaseInfo Instance {
            get {
                return _instance.Value;
            }
        }
        #endregion

        #region [1st class variable]
        private readonly Dictionary<string, Func<string, IDbConnection>> _connectionMaps = new() {
            {
                ENUM_DATABASE_TYPE.MSSQL.Value, connectionString => {
                    SqlServerBootstrap.Initialize();
                    return new SqlConnection(DbConnectionProvider.Instance.MSSQL);
                }
            }, {
                ENUM_DATABASE_TYPE.MYSQL.Value, connectionString => {
                    MySqlBootstrap.Initialize();
                    return new MySqlConnection(DbConnectionProvider.Instance.MYSQL);
                }
            }, {
                ENUM_DATABASE_TYPE.POSTGRESQL.Value, connectionString => {
                    PostgreSqlBootstrap.Initialize();
                    return new NpgsqlConnection(DbConnectionProvider.Instance.POSTGRESQL);
                }
            },
            // {
            //     "ORACLE", connectionString => {
            //         return new OracleConnection(connectionString);
            //     }
            // },
        };
        #endregion
        
        public Dictionary<string, IDbConnection> Connections { get; } =
            new();

        #region [ctor]
        public JDatabaseInfo() {
            var serviceCollection = new ServiceCollection();
            _connectionMaps.xForEach((item, index) => {
                Connections.Add(item.Key, item.Value(item.Key));
                return true;
            });
        }
        #endregion
    }
}
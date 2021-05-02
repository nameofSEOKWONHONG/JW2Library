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
        private readonly Dictionary<string, Func<string, IDbConnection>> _connectionMaps = new() {
            {
                "MSSQL", connectionString => {
                    SqlServerBootstrap.Initialize();
                    return new SqlConnection(DbConnectionProvider.Instance.MSSQL);
                }
            }, {
                "MYSQL", connectionString => {
                    MySqlBootstrap.Initialize();
                    return new MySqlConnection(DbConnectionProvider.Instance.MYSQL);
                }
            }, {
                "POSTGRESQL", connectionString => {
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

        public JDatabaseInfo() {
            var serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }

        public Dictionary<string, IDbConnection> Connections { get; } =
            new();

        private void InitConfig(IServiceCollection serviceCollection) {
            _connectionMaps.xForEach((item, index) => {
                Connections.Add(item.Key, item.Value(item.Key));
                return true;
            });
        }
    }
}
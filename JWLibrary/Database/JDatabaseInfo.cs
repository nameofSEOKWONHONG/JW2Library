using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JWLibrary.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
                    return new SqlConnection(connectionString);
                }
            }, {
                "MYSQL", connectionString => {
                    MySqlBootstrap.Initialize();
                    return new MySqlConnection(connectionString);
                }
            }, {
                "NPGSQL", connectionString => {
                    PostgreSqlBootstrap.Initialize();
                    return new NpgsqlConnection(connectionString);
                }
            },
            // {
            //     "ORACLE", connectionString => {
            //         return new OracleConnection(connectionString);
            //     }
            // },
        };

        private IConfiguration _configuration;

        public JDatabaseInfo() {
            var serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }

        public Dictionary<string, IDbConnection> Connections { get; } =
            new();

        private void InitConfig(IServiceCollection serviceCollection) {
            // Build configuration
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var section = _configuration.GetSection("DbConnections");

            _connectionMaps.forEach((item, index) => {
                if (section.GetValue<string>(item.Key).isNullOrEmpty()) return true;
                Connections.Add(item.Key, item.Value(section.GetValue<string>(item.Key)));
                return true;
            });
        }
    }
}
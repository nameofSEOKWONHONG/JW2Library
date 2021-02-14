using System;
using JWLibrary.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace JWLibrary.Database {

    /// <summary>
    ///     Database Information Master
    /// </summary>
    internal class JDatabaseInfo {
        private IConfiguration _configuration;
        private readonly Dictionary<string, Func<string, IDbConnection>> _connectionMaps = new Dictionary<string, Func<string, IDbConnection>>
        {
            {"MSSQL", (connectionString) => {
                    RepoDb.SqlServerBootstrap.Initialize();
                    return new SqlConnection(connectionString);
                }
            },
            {"MYSQL", (connectionString) => {
                    RepoDb.MySqlBootstrap.Initialize();
                    return new MySqlConnection(connectionString);
                }
            },
            {"NPGSQL", (connectionString) => {
                    RepoDb.PostgreSqlBootstrap.Initialize();
                    return new NpgsqlConnection(connectionString);
                }
            }
        };

        public Dictionary<string, IDbConnection> Connections { get; private set; } =
            new Dictionary<string, IDbConnection>();

        public JDatabaseInfo() {
            var serviceCollection = new ServiceCollection();
            InitConfig(serviceCollection);
        }
        
        private void InitConfig(IServiceCollection serviceCollection) {
            // Build configuration
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var section = _configuration.GetSection("DbConnections");

            _connectionMaps.jForEach(item => {
                if (section.GetValue<string>(item.Key).jIsNullOrEmpty()) return true;
                Connections.Add(item.Key, item.Value(section.GetValue<string>(item.Key)));
                return true;
            });
        }
    }
}
using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JWLibrary.Database {
    public class SqlKataCompilerFactory {
        private SqlKataCompilerFactory() {
        }

        public static QueryFactory CreateInstance(IDbConnection connection) {
            if (connection == typeof(MySqlConnection)) {
                var compiler = new MySqlCompiler();
                return new QueryFactory(connection, compiler);
            }
            else if (connection.GetType() == typeof(NpgsqlConnection)) {
                var compiler = new PostgresCompiler();
                return new QueryFactory(connection, compiler);
            }
            else if(connection.GetType() == typeof(Microsoft.Data.SqlClient.SqlConnection)) {
                var compiler = new SqlServerCompiler();
                return new QueryFactory(connection, compiler);
            }

            return null;
        }
    }
}
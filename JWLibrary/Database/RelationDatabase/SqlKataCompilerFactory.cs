using System.Data;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JWLibrary.Database
{
    internal class SqlKataCompilerFactory
    {
        private SqlKataCompilerFactory()
        {
        }

        public static QueryFactory CreateInstance(IDbConnection connection)
        {
            if (connection.GetType() == typeof(MySqlConnection))
            {
                var compiler = new MySqlCompiler();
                return new QueryFactory(connection, compiler);
            }

            if (connection.GetType() == typeof(NpgsqlConnection))
            {
                var compiler = new PostgresCompiler();
                return new QueryFactory(connection, compiler);
            }

            if (connection.GetType() == typeof(SqlConnection))
            {
                var compiler = new SqlServerCompiler();
                return new QueryFactory(connection, compiler);
            }

            return null;
        }
    }
}
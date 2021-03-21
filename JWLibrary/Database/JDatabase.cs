using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;

namespace JWLibrary.Database {
    public class JDataBase {
        private static readonly Lazy<JDatabaseInfo> JDataBaseInfo = new(() => { return new JDatabaseInfo(); });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection))
                return JDataBaseInfo.Value.Connections["MSSQL"];
            if (typeof(TDatabase) == typeof(MySqlConnection))
                return JDataBaseInfo.Value.Connections["MYSQL"];
            if (typeof(TDatabase) == typeof(NpgsqlConnection))
                return JDataBaseInfo.Value.Connections["NPGSQL"];
            throw new NotImplementedException();
        }

        public static Tuple<IDbConnection, IDbConnection> Resolve<TDatabaseA, TDatabaseB>()
            where TDatabaseA : IDbConnection
            where TDatabaseB : IDbConnection {
            if (typeof(TDatabaseA) == typeof(TDatabaseB)) throw new Exception("not allow same database connection.");
            return new Tuple<IDbConnection, IDbConnection>(Resolve<TDatabaseA>(), Resolve<TDatabaseB>());
        }
    }
}
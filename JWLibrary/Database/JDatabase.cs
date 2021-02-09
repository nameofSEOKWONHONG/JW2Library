using JWLibrary.Core;
using LiteDB;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace JWLibrary.Database {

    public class JDataBase {

        private static readonly Lazy<JDatabaseInfo> JDataBaseInfo = new Lazy<JDatabaseInfo>(() => {
            return new JDatabaseInfo();
        });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection))
                return JDataBaseInfo.Value.Connections["MSSQL"];
            else if (typeof(TDatabase) == typeof(MySqlConnection))
                return JDataBaseInfo.Value.Connections["MYSQL"];
            else if (typeof(TDatabase) == typeof(Npgsql.NpgsqlConnection))
                return JDataBaseInfo.Value.Connections["NPGSQL"];
            throw new NotImplementedException();
        }
    }
}
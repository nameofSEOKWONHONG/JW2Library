using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySql.Data.MySqlClient;
using Npgsql;

namespace JWLibrary.Database {
    /// <summary>
    /// create database connection Resolver 
    /// </summary>
    public class JDatabaseResolver {
        private static readonly Lazy<JDatabaseInfo> JDataBaseInfo = new(() => { return new JDatabaseInfo(); });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection)) return JDataBaseInfo.Value.Connections["MSSQL"];
            if (typeof(TDatabase) == typeof(MySqlConnection)) return JDataBaseInfo.Value.Connections["MYSQL"];
            if (typeof(TDatabase) == typeof(NpgsqlConnection)) return JDataBaseInfo.Value.Connections["POSTGRESQL"];
            throw new NotImplementedException();
        }

        public static (IDbConnection first, IDbConnection second) Resolve<TDatabaseA, TDatabaseB>()
            where TDatabaseA : IDbConnection
            where TDatabaseB : IDbConnection {
            if (typeof(TDatabaseA) == typeof(TDatabaseB)) throw new Exception("not allow same database connection.");
            return new ValueTuple<IDbConnection, IDbConnection>(Resolve<TDatabaseA>(), Resolve<TDatabaseB>());
        }
    }
}
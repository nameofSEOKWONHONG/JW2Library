﻿using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Connections;
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

        public static (IDbConnection, IDbConnection) Resolve<TDatabaseA, TDatabaseB>()
            where TDatabaseA : IDbConnection
            where TDatabaseB : IDbConnection {
            if (typeof(TDatabaseA) == typeof(TDatabaseB)) throw new Exception("not allow same database connection.");
            return new (Resolve<TDatabaseA>(), Resolve<TDatabaseB>());
        }
    }

    public class JSqlEfDbContext: DbContext {
        private readonly string _connectionString;

        public JSqlEfDbContext() {
        }

        public JSqlEfDbContext(string connectionString) {
            this._connectionString = connectionString;
        }

        public JSqlEfDbContext(DbContextOptions<JSqlEfDbContext> options) : base(options) {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_connectionString);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class JMySqlEfDbContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class T2 {
        public void Run() {
            var context = new DbContextOptionsBuilder<JSqlEfDbContext>()
                .UseSqlServer("").Options;

        }
    }
}
﻿using System;
using System.Threading.Tasks;
using eXtensionSharp;
using LiteDB;
using MongoDB.Driver;
using StackExchange.Redis;
using ConnectionType = LiteDB.ConnectionType;

namespace JWLibrary.Database {
    public class MongoClientHandler {
        private readonly IMongoClient _client;
        private IMongoDatabase _mongoDatabase;

        public MongoClientHandler(string connnectionString, string database) {
            _client = new MongoClient(connnectionString);
            _mongoDatabase = _client.GetDatabase(database);
        }

        public void Execute<T>(string table, Action<IMongoCollection<T>> execute) {
            var collection = _mongoDatabase.GetCollection<T>(table);
            execute(collection);
        }

        public async Task<bool> ExecuteAsync<T>(string table, Func<IMongoCollection<T>, Task<bool>> executeAsync) {
            var collection = _mongoDatabase.GetCollection<T>(table);
            var @is = await executeAsync(collection);
            return @is;
        }
    }

    public class RedisClientHandler {
        private readonly ConnectionMultiplexer _muxer;
        private IDatabase _database;

        public RedisClientHandler(string connectionString) {
            _muxer = ConnectionMultiplexer.Connect(connectionString);
            _database = _muxer.GetDatabase();
        }

        public void Execute(Action<IDatabase> execute) {
            execute(_database);
        }

        public async Task<bool> ExecuteAsync(Func<IDatabase, Task<bool>> executeAsync) {
            return await executeAsync(_database);
        } 
    }

    /// <summary>
    /// litedb handler. dont'use this. replace JLiteDbFlex
    /// </summary>
    [Obsolete("Don't use", true)]
    public class LiteDbHandler : IDisposable {
        private LiteDB.LiteDatabase _database;
        

        public LiteDbHandler(ConnectionString connectionString) {
            connectionString.Connection = ConnectionType.Shared;
            _database = new LiteDatabase(connectionString);
        }

        public void Execute<T>(string table, Action<ILiteCollection<T>> action)
            where T : class {
            var col = _database.GetCollection<T>(table);
            action(col);
        }

        public async Task<bool> ExecuteAsync<T>(string table, Func<ILiteCollection<T>, Task<bool>> func) 
            where T : class {
            var col = _database.GetCollection<T>(table);
            var result = await func(col);
            return result;
        }


        public void Dispose() {
            _database?.Dispose();
        }
    }
}
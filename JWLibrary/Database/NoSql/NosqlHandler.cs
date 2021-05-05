using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using eXtensionSharp;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;

namespace JWLibrary.Database {
    public class MongoClientHandler {
        private IMongoDatabase _mongoDatabase;

        private readonly string _table = null;

        public MongoClientHandler(string connnectionString, string database, string table = null) {
            if (database.xIsNullOrEmpty()) throw new Exception("database is empty.");
            _table = table;
            IMongoClient client = new MongoClient(connnectionString);
            _mongoDatabase = client.GetDatabase(database);
        }

        public void Execute<T>(Action<IMongoCollection<BsonDocument>> execute) {
            var collection = _mongoDatabase.GetCollection<BsonDocument>(typeof(T).Name);
            execute(collection);
        }
        
        public void Execute<T>(Action<IMongoCollection<T>> execute) {
            var collection = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            execute(collection);
        }

        public void Execute(Action<IMongoCollection<BsonDocument>> execute) {
            if (_table.xIsNullOrEmpty()) throw new Exception("table is empty.");
            var collection = _mongoDatabase.GetCollection<BsonDocument>(_table);
            execute(collection);
        }

        public async Task<bool> ExecuteAsync<T>(string table, Func<IMongoCollection<T>, Task<bool>> executeAsync) {
            var collection = _mongoDatabase.GetCollection<T>(table);
            var @is = await executeAsync(collection);
            return @is;
        }
    }

    public class RedisClientHandler {
        private IDatabase _database;

        public RedisClientHandler(string connectionString) {
            var muxer = ConnectionMultiplexer.Connect(connectionString);
            _database = muxer.GetDatabase();
        }

        public void Execute(Action<IDatabase> execute) {
            execute(_database);
        }

        public async Task<bool> ExecuteAsync(Func<IDatabase, Task<bool>> executeAsync) {
            return await executeAsync(_database);
        } 
    }
}
using System;
using System.Threading.Tasks;
using JWLibrary.Core;
using MongoDB.Driver;
using StackExchange.Redis;

namespace JWLibrary.Database {
    public class MongoClientHandler {
        private readonly IMongoClient _client;
        private IMongoDatabase _mongoDatabase;

        public MongoClientHandler(string connnectionString) {
            _client = new MongoClient(connnectionString);
        }

        public void open(string database = null) {
            _mongoDatabase = _client.GetDatabase(database);
        }

        public void handle<T>(string table, Action<IMongoCollection<T>> execute) {
            var collection = _mongoDatabase.GetCollection<T>(table);
            execute(collection);
        }

        public async Task<bool> handleAsync<T>(string table, Func<IMongoCollection<T>, Task<bool>> executeAsync) {
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
        }

        public void open() {
            if(_muxer.IsConnected)
                _database = _muxer.GetDatabase();
        }

        public void handle(Action<IDatabase> execute) {
            execute(_database);
        }
    }

    public class NoSqlHandlerTest {
        public void Run() {
            var testobj = new Test();
            testobj.Name = "test";
            testobj.Age = 10;
            RedisClientHandler handler = new RedisClientHandler("ip");
            handler.open();
            handler.handle(db => {
                var result = db.SetAdd("test", testobj.fromObjectToJson());
                Console.WriteLine(result);
            });
            handler.handle(db => {
                var result = db.StringGet("test");
                Console.WriteLine(result);
            });
        }

        public void Run2() {
            MongoClientHandler handler = new MongoClientHandler("ip");
            handler.open("test");
            handler.handle<Test>("test", col => {
                col.InsertOne(new Test() {
                    Name = "test",
                    Age = 10
                });
            });
            handler.handle<Test>("test", col => {
                var result = col.Find(m => m.Name == "test").First();
                Console.WriteLine(result.fromObjectToJson());
            });
        }
    }

    public class Test {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
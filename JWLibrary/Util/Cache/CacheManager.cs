using System;
using System.IO;
using eXtensionSharp;
using LiteDB;
using Newtonsoft.Json;

namespace JWLibrary.Util.Cache {
    public interface ICacheManager : IDisposable {
        T Get<T>(object obj);
    }
    
    public class CacheManager : ICacheManager {
        private const string PREFIX_DIR = "Cache";
        private const string CACHE_DB_FILE_NAME = "cache.db";
        private const string CACHE_DB_TABLE_NAME = "cacheinfo";
        private readonly LiteDatabase _liteDatabase;

        public CacheManager() {
            if (!Directory.Exists(PREFIX_DIR.xToPath())) Directory.CreateDirectory(PREFIX_DIR.xToPath());
            _liteDatabase = new LiteDatabase($"{PREFIX_DIR}/{CACHE_DB_FILE_NAME}");
            var col = _liteDatabase.GetCollection(CACHE_DB_TABLE_NAME);
            col.EnsureIndex("Key");
        }

        public void Dispose() {
            _liteDatabase.Dispose();
        }

        ~CacheManager() {
            Dispose();
        }

        public T Get<T>(object obj) {
            T result;
            GetOrAdd(obj, out result);
            return result;
        }

        private void GetOrAdd<T>(object obj, out T result) {
            result = default;

            var col = _liteDatabase.GetCollection(CACHE_DB_TABLE_NAME);
            var cacheKey = obj.GetHashCode().ToString();
            var exists = col.FindOne($"$.Key='{cacheKey}'");

            if (exists == null) {
                var cacheInfo = new CacheInfo {
                    Key = obj.GetHashCode().ToString(),
                    Value = JsonConvert.SerializeObject(obj)
                };
                var bsonDoc = BsonMapper.Global.ToDocument(cacheInfo);
                col.Insert(bsonDoc);
            }
            else {
                result = exists["Value"].AsString.xJsonToObject<T>();
            }
        }
    }

    public class CacheInfo {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
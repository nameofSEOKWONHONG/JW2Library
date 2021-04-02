using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Community.CsharpSqlite;
using JLiteDBFlex;
using JWLibrary.Core;
using LiteDB;
using Newtonsoft.Json;
using JsonSerializer = LiteDB.JsonSerializer;

namespace JWLibrary.Util.Cache {
    public class CacheManager : IDisposable{
        private readonly LiteDatabase _liteDatabase;
        private const string CACHE_DB_FILE_NAME = "./Cache/cache.db";
        private const string CACHE_DB_TABLE_NAME = "cacheinfo";
        public CacheManager(string alias = null) {
            _liteDatabase = new LiteDatabase(CACHE_DB_FILE_NAME);
            var col = _liteDatabase.GetCollection(CACHE_DB_TABLE_NAME);
            col.EnsureIndex("Key");
        }

        ~CacheManager() {
            Dispose();
        }

        public T Get<T>(object obj) {
            var result = default(T);
            this.GetOrAdd<T>(obj, out result);
            return result;
        }

        private void GetOrAdd<T>(object obj, out T result) {
            result = default(T);
            
            var col = _liteDatabase.GetCollection(CACHE_DB_TABLE_NAME);
            var cacheKey = obj.GetHashCode().ToString();
            var exists = col.FindOne($"$.Key='{cacheKey}'");

            if (exists == null) {
                var cacheInfo = new CacheInfo() {
                    Key = obj.GetHashCode().ToString(),
                    Value = JsonConvert.SerializeObject(obj)
                };
                var bsonDoc = BsonMapper.Global.ToDocument(cacheInfo);
                col.Insert(bsonDoc);
            }
            else {
                result = JsonConvert.DeserializeObject<T>(exists["Value"].AsString);
            }
        }

        public void Dispose() {
            _liteDatabase.Dispose();
        }
    }

    public class CacheInfo {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
using System;
using System.Collections.Concurrent;
using eXtensionSharp;
using JWLibrary.Database;
using Org.BouncyCastle.Asn1.Cms;
using StackExchange.Redis;

namespace JWLibrary.Util.Cache {
    public class CacheHandler {
        private readonly RedisClientHandler _redisClientHandler;

        public CacheHandler() {
            _redisClientHandler = new RedisClientHandler(NoSqlConnectionProvider.Instance.REDIS);
        }

        public CacheValue<T> Get<T>(string key) {
            CacheValue<T> exists = null;
            _redisClientHandler.Execute(db => {
                var redisValue = db.StringGet(key);
                if (redisValue.HasValue) {
                    exists = redisValue.ToString().xJsonToObject<CacheValue<T>>();    
                }
            });
            return exists;
        }

        public bool Add<T>(string key, T addItem) {
            var cacheValue = new CacheValue<T>(addItem);
            var result = false;
            _redisClientHandler.Execute(db => {
                result = db.StringSet(key, cacheValue.xObjectToJson());
            });

            //if return value is false, exists cache item;
            return result;
        }

        public bool Delete(string key) {
            var result = false;
            _redisClientHandler.Execute(db => {
                result = db.KeyDelete(key);
            });
            return result;
        }

        public int Count() {
            var cnt = 0;
            _redisClientHandler.Execute(db => {
                var arr = (RedisResult[])db.Execute("scan", 0);
                cnt = arr.Length;
            });
            return cnt;
        }
    }
    
    public class CacheValue<T> {
        public T ValueObject { get; set; }
        public DateTime CachedDateTime { get; set; }

        public CacheValue(T value) {
            this.ValueObject = value;
            this.CachedDateTime = DateTime.Now;
        }
    }   
}
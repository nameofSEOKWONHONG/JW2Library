using eXtensionSharp;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace JWLibrary.Database {
    public static class RedisExtension {
        public static bool Set<T>(this IDatabase db, string key, T obj) {
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) {
                return db.StringSet(key, obj.xSafe());
            }
            var json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json);
        }

        public static T Get<T>(this IDatabase db, string key) {
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) {
                return db.StringGet(key).xSafe<T>();
            }
            var exists = db.StringGet(key);
            if (exists.HasValue.xIsTrue()) {
                var obj = JsonConvert.DeserializeObject<T>(exists);
                return obj;
            }

            return default;
        }
    }
}
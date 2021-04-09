using System.Collections.Generic;
using Newtonsoft.Json;

namespace JWLibrary.Core {
    public static class JSerializer {
        public static T fromJsonToObject<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static IEnumerable<T> fromJsonToObjects<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public static string fromObjectToJson<T>(this T entity, Formatting? formatting = null,
            JsonSerializerSettings serializerSettings = null)
            where T : class {
            if (formatting.isNotNull() && serializerSettings.isNotNull())
                return JsonConvert.SerializeObject(entity, formatting.Value, serializerSettings);
            if (formatting.isNotNull() && serializerSettings.isNull())
                return JsonConvert.SerializeObject(entity, formatting.Value);
            if (formatting.isNull() && serializerSettings.isNotNull())
                return JsonConvert.SerializeObject(entity, serializerSettings);
            return JsonConvert.SerializeObject(entity);
        }

        public static string fromObjectToJson<TKey, TValue>(this JDictionaryPool<TKey, TValue> dictionaryPool) {
            var dic = dictionaryPool.ToDictionary();
            return JsonConvert.SerializeObject(dic);
        }

        public static string fromObjectToJson(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JWLibrary.Core {
    public static class JSerializer {
        public static T jJsonToObject<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static IEnumerable<T> jJsonToObjects<T>(this string jsonString) {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public static string jObjectToJson<T>(this T entity, Formatting? formatting = null,
            JsonSerializerSettings serializerSettings = null)
            where T : class {
            if (formatting.jIsNotNull() && serializerSettings.jIsNotNull())
                return JsonConvert.SerializeObject(entity, formatting.Value, serializerSettings);
            if (formatting.jIsNotNull() && serializerSettings.jIsNull())
                return JsonConvert.SerializeObject(entity, formatting.Value);
            if (formatting.jIsNull() && serializerSettings.jIsNotNull())
                return JsonConvert.SerializeObject(entity, serializerSettings);
            return JsonConvert.SerializeObject(entity);
        }

        public static string jObjectToJson<TKey, TValue>(this JDictionaryPool<TKey, TValue> dictionaryPool) {
            var dic = dictionaryPool.ToDictionary();
            return JsonConvert.SerializeObject(dic);
        }

        public static string jObjectToJson(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
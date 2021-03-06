using System;
using System.Linq;
using JWLibrary.Core;

namespace JLiteDBFlex {
    public class JLiteDbFlexerManager<T> where T : class {
        private static Lazy<JHDictionary<Type, JLiteDbFlexer<T>>> _instance =
            new Lazy<JHDictionary<Type, JLiteDbFlexer<T>>>();
        
        public static JLiteDbFlexer<T> Create() {
            var exists = _instance.Value.FirstOrDefault(m => m.Key == typeof(T));
            if (exists.jIsNotNull()) {
                return exists.Value;
            }
            
            var newInstance = new JLiteDbFlexer<T>();
            if (newInstance.jIsNotNull()) {
                _instance.Value.Add(typeof(T), newInstance);
            }

            return newInstance;
        }
    }
}
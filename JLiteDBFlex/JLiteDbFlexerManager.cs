using System;
using System.Linq;
using JWLibrary.Core;
using Nito.AsyncEx;

namespace JLiteDBFlex {
    public class JLiteDbFlexerManager {
        private static readonly AsyncLock _mutex = new AsyncLock();
        private static JHDictionary<Type, IJLiteDbFlexer> _instanceMap =
            new JHDictionary<Type, IJLiteDbFlexer>();
        
        public static JLiteDbFlexer<T> Create<T>() where T : class {
            using (_mutex.Lock()) {
                var exists = _instanceMap.FirstOrDefault(m => m.Key == typeof(T));
                if (exists.jIsNotNull()) {
                    return exists.Value as JLiteDbFlexer<T>;
                }
            
                var newInstance = new JLiteDbFlexer<T>();
                if (newInstance.jIsNotNull()) {
                    _instanceMap.Add(typeof(T), newInstance);
                }

                return newInstance;
            }
        }

        public static void Distroy() {
            _instanceMap.jForEach(instance => {
                if (instance.Value.jIsNotNull()) {
                    instance.Value.LiteDatabase?.Dispose();
                }
            });

            _instanceMap.Clear();
        }
    }
}
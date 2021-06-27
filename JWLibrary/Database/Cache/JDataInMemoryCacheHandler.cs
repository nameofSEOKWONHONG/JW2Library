using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using eXtensionSharp;

namespace JWLibrary.Database {
    internal class JDataInMemoryCacheHandler {
        private static Lazy<JDataInMemoryCacheHandler> _instance = new Lazy<JDataInMemoryCacheHandler>(() => new JDataInMemoryCacheHandler());

        public static JDataInMemoryCacheHandler Instance {
            get { return _instance.Value; }
        }

        private ConcurrentDictionary<string, object> _caches = new ConcurrentDictionary<string, object>();
        
        private JDataInMemoryCacheHandler() {
            
        }

        public TResult GetOrAdd<TKey, TResult>(TKey key, TResult result) {
            return (TResult)_caches.GetOrAdd(key.xObjectToJson(), result);
        }

        public void ResetCache<TKey>(TKey key) {
            object o = null;
            if (!_caches.TryRemove(key.xObjectToJson(), out o)) {
                Trace.WriteLine($"{key.xObjectToJson()} not deleted");
            }
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using eXtensionSharp;

namespace RocksGrpcNetClient {
    internal class JInMemoryCacheHandler {
        private static Lazy<JInMemoryCacheHandler> _instance = new Lazy<JInMemoryCacheHandler>(() => new JInMemoryCacheHandler());

        public static JInMemoryCacheHandler Instance {
            get { return _instance.Value; }
        }

        private ConcurrentDictionary<string, object> _caches = new ConcurrentDictionary<string, object>();
        
        private JInMemoryCacheHandler() {
            
        }

        public TResult GetOrAdd<TKey, TResult>(TKey key, TResult result) {
            return (TResult)_caches.GetOrAdd(key.xToJson(), result);
        }

        public void ResetCache<TKey>(TKey key) {
            object o = null;
            if (!_caches.TryRemove(key.xToJson(), out o)) {
                Trace.WriteLine($"{key.xToJson()} not deleted");
            }
        }
    }
}
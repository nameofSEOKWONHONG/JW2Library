using System;

namespace RocksGrpcNetClient {
    public class JCacheHandler {
        private static readonly Lazy<JCacheHandler> _instance = new(() => new JCacheHandler());

        private JCacheHandler() {
        }

        public static JCacheHandler Instance => _instance.Value;

        public TResult GetOrAdd<TKey, TResult>(TKey key, TResult result, ENUM_CACHE_TYPE type) {
            if (type == ENUM_CACHE_TYPE.IN_MEMORY)
                return JInMemoryCacheHandler.Instance.GetOrAdd(key, result);
            if (type == ENUM_CACHE_TYPE.ROCKSDB) return JDataGrpcRocksDBCacheHandler.Instance.GetOrAdd(key, result);

            throw new NotImplementedException();
        }

        public void ResetCache<TKey>(TKey key, ENUM_CACHE_TYPE type) {
            if (type == ENUM_CACHE_TYPE.IN_MEMORY) {
                JInMemoryCacheHandler.Instance.ResetCache(key);
                return;
            }

            if (type == ENUM_CACHE_TYPE.ROCKSDB) {
                JDataGrpcRocksDBCacheHandler.Instance.ResetCache(key);
                return;
            }

            throw new NotImplementedException();
        }
    }
}
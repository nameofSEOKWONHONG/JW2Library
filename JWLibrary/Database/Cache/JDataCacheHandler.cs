using System;

namespace JWLibrary.Database {
    public class JDataCacheHandler {
        private static readonly Lazy<JDataCacheHandler> _instance = new(() => new JDataCacheHandler());

        private JDataCacheHandler() {
        }

        public static JDataCacheHandler Instance => _instance.Value;

        public TResult GetOrAdd<TKey, TResult>(TKey key, TResult result, ENUM_CACHE_TYPE type) {
            if (type == ENUM_CACHE_TYPE.IN_MEMORY)
                return JDataInMemoryCacheHandler.Instance.GetOrAdd(key, result);
            if (type == ENUM_CACHE_TYPE.ROCKSDB) return JDataGrpcRocksDBCacheHandler.Instance.GetOrAdd(key, result);

            throw new NotImplementedException();
        }

        public void ResetCache<TKey>(TKey key, ENUM_CACHE_TYPE type) {
            if (type == ENUM_CACHE_TYPE.IN_MEMORY) {
                JDataInMemoryCacheHandler.Instance.ResetCache(key);
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
using eXtensionSharp;

namespace RocksGrpcNetClient{
    public class ENUM_CACHE_TYPE : XEnumBase<ENUM_CACHE_TYPE> {
        public static readonly ENUM_CACHE_TYPE IN_MEMORY = Define("IN_MEMORY");
        public static readonly ENUM_CACHE_TYPE ROCKSDB = Define("ROCKSDB");
    }
}
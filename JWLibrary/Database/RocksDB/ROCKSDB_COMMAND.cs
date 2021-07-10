using eXtensionSharp;

namespace JWLibrary.Database
{
    public class ROCKSDB_COMMAND : XEnumBase<ROCKSDB_COMMAND>
    {
        public static readonly ROCKSDB_COMMAND Put = Define("PUT");
        public static readonly ROCKSDB_COMMAND Puts = Define("PUTS");
        public static readonly ROCKSDB_COMMAND Get = Define("GET");
        public static readonly ROCKSDB_COMMAND Gets = Define("GETS");
        public static readonly ROCKSDB_COMMAND Remove = Define("REMOVE");
        public static readonly ROCKSDB_COMMAND Removes = Define("REMOVES");
    }
}
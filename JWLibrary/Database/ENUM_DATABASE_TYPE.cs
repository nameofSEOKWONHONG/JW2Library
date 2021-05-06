using eXtensionSharp;

namespace JWLibrary.Database {
    public class ENUM_DATABASE_TYPE : XENUM_BASE<ENUM_DATABASE_TYPE> {
        public static readonly ENUM_DATABASE_TYPE MSSQL = Define("MSSQL");
        public static readonly ENUM_DATABASE_TYPE MYSQL = Define("MYSQL");
        public static readonly ENUM_DATABASE_TYPE POSTGRESQL = Define("POSTGRESQL");
        public static readonly ENUM_DATABASE_TYPE REDIS = Define("REDIS");
        public static readonly ENUM_DATABASE_TYPE MONGODB = Define("MONGODB");
        public static readonly ENUM_DATABASE_TYPE SQLITE = Define("SQLITE");
        public static readonly ENUM_DATABASE_TYPE SQLITE_IN_MEMORY = Define("SQLITE_IN_MEMORY");
        public static readonly ENUM_DATABASE_TYPE LITEDB = Define("LITEDB");
    }
}
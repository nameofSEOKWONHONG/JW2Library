using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using eXtensionSharp;
using JWLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Nito.AsyncEx;

namespace JWLibrary.Database {
    public class DbConnectionProvider {
        private static Lazy<DbConnectionProvider> _dbconnectionProvider =
            new Lazy<DbConnectionProvider>(() => new DbConnectionProvider());

        public static DbConnectionProvider Instance {
            get {
                return _dbconnectionProvider.Value;
            }
        }
        
        public readonly string MSSQL = ProviderMaps[ENUM_DATABASE_TYPE.MSSQL.Value].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string MYSQL = ProviderMaps[ENUM_DATABASE_TYPE.MYSQL.Value].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE = ProviderMaps[ENUM_DATABASE_TYPE.SQLITE.Value].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE_IN_MEMORY = ProviderMaps[ENUM_DATABASE_TYPE.SQLITE_IN_MEMORY.Value].xToDecAes256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string POSTGRESQL = ProviderMaps[ENUM_DATABASE_TYPE.POSTGRESQL.Value].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public DbConnectionProvider() {
            
        }

        private static Dictionary<string, string> _providerMaps;
        private static AsyncLock _mutex = new AsyncLock();
        
        private static Dictionary<string, string> ProviderMaps {
            get {
                if (_providerMaps.xIsNull()) {
                    using (_mutex.Lock()) {
                        if (_providerMaps.xIsNull()) {
                            _providerMaps = new Dictionary<string, string>();
                            var configFile = @"D:\workspace\JW2Library\JConfiguration\jconfig.json";
                            var configJson = configFile.xFileReadLine();

                            var jconfig = configJson.xJsonToObject<JConfig>();
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MSSQL.Value, jconfig.DatabaseProvider.MSSQL);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MYSQL.Value, jconfig.DatabaseProvider.MYSQL);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.SQLITE.Value, jconfig.DatabaseProvider.SQLITE);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.SQLITE_IN_MEMORY.Value, jconfig.DatabaseProvider.SQLITE_IN_MEMORY);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.POSTGRESQL.Value, jconfig.DatabaseProvider.POSTGRESQL);    
                            _providerMaps.Add(ENUM_DATABASE_TYPE.REDIS.Value, jconfig.DatabaseProvider.REDIS);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MONGODB.Value, jconfig.DatabaseProvider.MONGODB);
                        }
                    }
                }

                return _providerMaps;
            }
        }
    }
    
    public class JConfig {
        public JDatabaseProviderConfig DatabaseProvider { get; set; }
    }

    public class JDatabaseProviderConfig {
        public string MSSQL { get; set; }
        public string MYSQL { get; set; }
        public string POSTGRESQL { get; set; }
        public string MONGODB { get; set; }
        public string REDIS { get; set; }
        public string SQLITE { get; set; }
        public string SQLITE_IN_MEMORY { get; set; }
        public string KEY { get; set; }
        public string CHIPER { get; set; }
    }
}
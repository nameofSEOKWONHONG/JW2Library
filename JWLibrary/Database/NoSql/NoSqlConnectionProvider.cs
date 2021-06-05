using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using eXtensionSharp;
using JWLibrary.Utils;
using JWLibrary.Web.Consts;
using Microsoft.Extensions.Configuration;
using Nito.AsyncEx;

namespace JWLibrary.Database {
    public class NoSqlConnectionProvider {
        private static Lazy<NoSqlConnectionProvider> _nosqlConnectionProdiver =
            new Lazy<NoSqlConnectionProvider>(() => new NoSqlConnectionProvider());

        public static NoSqlConnectionProvider Instance {
            get {
                return _nosqlConnectionProdiver.Value;
            }
        }
        
        public readonly string REDIS = ProviderMaps["REDIS"].xToDecAES256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
        
        public readonly string MONGODB = ProviderMaps["MONGODB"].xToDecAES256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);        
        
        //public readonly string REDIS = ProviderMaps["REDIS"]
        
        private static Dictionary<string, string> _providerMaps;
        private static AsyncLock _mutex = new(); 
        
        private static Dictionary<string, string> ProviderMaps {
            get {
                if (_providerMaps.xIsNull()) {
                    using (_mutex.Lock()) {
                        if (_providerMaps.xIsNull()) {
                            _providerMaps = new Dictionary<string, string>();
                            var configFile = CONFIG_CONST.DATABASE_CONFIG_PATH;
                            var configJson = configFile.xFileReadLine();
                            var jconfig = configJson.xJsonToObject<JConfig>();
                            _providerMaps.Add(ENUM_DATABASE_TYPE.REDIS.Value, jconfig.DatabaseProvider.REDIS);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MONGODB.Value, jconfig.DatabaseProvider.MONGODB);
                        }
                    }
                }

                return _providerMaps;
            }
        }        
    }
}
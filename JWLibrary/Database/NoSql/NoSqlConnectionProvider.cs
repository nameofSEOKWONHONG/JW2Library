using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using eXtensionSharp;
using Nito.AsyncEx;

namespace JWLibrary.Database
{
    public class NoSqlConnectionProvider
    {
        private static readonly Lazy<NoSqlConnectionProvider> _nosqlConnectionProdiver =
            new(() => new NoSqlConnectionProvider());

        //public readonly string REDIS = ProviderMaps["REDIS"]

        private static Dictionary<string, string> _providerMaps;
        private static readonly AsyncLock _mutex = new();

        public readonly string MONGODB = ProviderMaps["MONGODB"].xToDecAES256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string REDIS = ProviderMaps["REDIS"].xToDecAES256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public static NoSqlConnectionProvider Instance => _nosqlConnectionProdiver.Value;

        private static Dictionary<string, string> ProviderMaps
        {
            get
            {
                if (_providerMaps.xIsNull())
                    using (_mutex.Lock())
                    {
                        if (_providerMaps.xIsNull())
                        {
                            _providerMaps = new Dictionary<string, string>();
                            var configFile = CONFIG_CONST.DATABASE_CONFIG_PATH;
                            var configJson = configFile.xFileReadLine();
                            var jconfig = configJson.xToEntity<JConfig>();
                            _providerMaps.Add(ENUM_DATABASE_TYPE.REDIS.ToString(), jconfig.DatabaseProvider.REDIS);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MONGODB.ToString(), jconfig.DatabaseProvider.MONGODB);
                        }
                    }

                return _providerMaps;
            }
        }
    }
}
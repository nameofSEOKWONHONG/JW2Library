using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using eXtensionSharp;
using Nito.AsyncEx;

namespace JWLibrary.Database
{
    public class DbConnectionProvider
    {
        private static readonly Lazy<DbConnectionProvider> _dbconnectionProvider =
            new(() => new DbConnectionProvider());

        private static Dictionary<string, string> _providerMaps;
        private static readonly AsyncLock _mutex = new();

        public readonly string MSSQL = ProviderMaps[ENUM_DATABASE_TYPE.MSSQL.ToString()].xToDecAES256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string MYSQL = ProviderMaps[ENUM_DATABASE_TYPE.MYSQL.ToString()].xToDecAES256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string POSTGRESQL = ProviderMaps[ENUM_DATABASE_TYPE.POSTGRESQL.ToString()].xToDecAES256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE = ProviderMaps[ENUM_DATABASE_TYPE.SQLITE.ToString()].xToDecAES256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE_IN_MEMORY = ProviderMaps[ENUM_DATABASE_TYPE.SQLITE_IN_MEMORY.ToString()]
            .xToDecAES256(
                DbCipherKeyIVProvider.Instance.Key,
                DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public static DbConnectionProvider Instance => _dbconnectionProvider.Value;

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
                            var configJson = configFile.xFileReadAllText();

                            var jconfig = configJson.xToEntity<JConfig>();
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MSSQL.ToString(), jconfig.DatabaseProvider.MSSQL);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.MYSQL.ToString(), jconfig.DatabaseProvider.MYSQL);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.SQLITE.ToString(), jconfig.DatabaseProvider.SQLITE);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.SQLITE_IN_MEMORY.ToString(),
                                jconfig.DatabaseProvider.SQLITE_IN_MEMORY);
                            _providerMaps.Add(ENUM_DATABASE_TYPE.POSTGRESQL.ToString(),
                                jconfig.DatabaseProvider.POSTGRESQL);
                        }
                    }

                return _providerMaps;
            }
        }
    }

    public class JConfig
    {
        public JDatabaseProviderConfig DatabaseProvider { get; set; }
    }

    public class JDatabaseProviderConfig
    {
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
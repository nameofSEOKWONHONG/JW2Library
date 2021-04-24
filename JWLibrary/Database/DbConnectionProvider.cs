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
        
        public readonly string MSSQL = ProviderMaps["MSSQL"].toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string MYSQL = ProviderMaps["MYSQL"].toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE = ProviderMaps["SQLITE"].toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string SQLITE_IN_MEMORY = ProviderMaps["SQLITE_IN_MEMORY"].toDecAes256(
            DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public readonly string NPGSQL = ProviderMaps["NPGSQL"].toDecAes256(DbCipherKeyIVProvider.Instance.Key,
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
                            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("./appsettings.json", true, true)
                                .AddEnvironmentVariables()
                                .Build();
                            var section = configuration.GetSection("DbConnectionProvider");
                            _providerMaps.Add("MSSQL", section.GetValue<string>("MSSQL"));
                            _providerMaps.Add("MYSQL", section.GetValue<string>("MYSQL"));
                            _providerMaps.Add("SQLITE", section.GetValue<string>("SQLITE"));
                            _providerMaps.Add("SQLITE_IN_MEMORY", section.GetValue<string>("SQLITE_IN_MEMORY"));
                            _providerMaps.Add("NPGSQL", section.GetValue<string>("NPGSQL"));                            
                        }
                    }
                }

                return _providerMaps;
            }
        }
    }
}
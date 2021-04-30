using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using eXtensionSharp;
using JWLibrary.Utils;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace JWLibrary.Database {
    public class NoSqlConnectionProvider {
        private static Lazy<NoSqlConnectionProvider> _nosqlConnectionProdiver =
            new Lazy<NoSqlConnectionProvider>(() => new NoSqlConnectionProvider());

        public static NoSqlConnectionProvider Instance {
            get {
                return _nosqlConnectionProdiver.Value;
            }
        }
        
        public readonly string REDIS = ProviderMaps["REDIS"].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
        
        public readonly string MONGODB = ProviderMaps["MONGODB"].xToDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);        
        
        //public readonly string REDIS = ProviderMaps["REDIS"]
        
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
                            var section = configuration.GetSection("NoSqlConnectionProvider");
                            _providerMaps.Add("REDIS", section.GetValue<string>("REDIS"));
                            _providerMaps.Add("MONGODB", section.GetValue<string>("MONGODB"));
                        }
                    }
                }

                return _providerMaps;
            }
        }        
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using JWLibrary.Utils;
using Microsoft.Extensions.Configuration;

namespace JWLibrary.EF {
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
        
        private static Dictionary<string, string> ProviderMaps {
            get {
                var maps = new Dictionary<string, string>();
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("./appsettings.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();
                var section = configuration.GetSection("DbConnectionProvider");
                maps.Add("MSSQL", section.GetValue<string>("MSSQL"));
                maps.Add("MYSQL", section.GetValue<string>("MYSQL"));
                maps.Add("SQLITE", section.GetValue<string>("SQLITE"));
                maps.Add("SQLITE_IN_MEMORY", section.GetValue<string>("SQLITE_IN_MEMORY"));
                maps.Add("NPGSQL", section.GetValue<string>("NPGSQL"));
                return maps; 
            }
        }
    }
}
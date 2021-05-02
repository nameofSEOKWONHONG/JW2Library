using System;
using eXtensionSharp;

namespace JWLibrary.Database {
    internal class DbCipherKeyIVProvider {
        private static readonly Lazy<DbCipherKeyIVProvider> _instance =
            new(() => new DbCipherKeyIVProvider());

        public DbCipherKeyIVProvider() {
            var keyiv = Get();
            Key = keyiv.key;
            IV = keyiv.iv;
        }

        public string Key { get; }
        public string IV { get; }

        public static DbCipherKeyIVProvider Instance => _instance.Value;

        public (string key, string iv) Get() {
            //setting key & iv, read file or http request
            var configFile = @"D:\workspace\JW2Library\JConfiguration\jconfig.json";
            var configJson = configFile.xFileReadLine();
            var jconfig = configJson.xJsonToObject<JConfig>();
            return new(jconfig.DatabaseProvider.KEY, jconfig.DatabaseProvider.CHIPER);
        }
    }
}
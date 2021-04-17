using System;

namespace JWLibrary.EF {
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
            return new("asdfasdfasdfasdf", "asdfasdfasdfasdf");
        }
    }
}
using System;
using System.Security.Cryptography;
using JWLibrary.Utils;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    /// <summary>
    ///     base db context
    /// </summary>
    public abstract class JEFDbContextBase : DbContext {
        protected JEFDbContextBase() {
        }

        protected JEFDbContextBase(DbContextOptions options) : base(options) {
        }
    }

    /// <summary>
    ///     mssql dbcontext
    /// </summary>
    public class JSqlDbContext : JEFDbContextBase {
        protected JSqlDbContext() {
        }

        protected JSqlDbContext(DbContextOptions options)
            : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(DbConnectionProvider.MSSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    /// <summary>
    ///     mysql dbcontext
    /// </summary>
    public class JMySqlDbContext : JEFDbContextBase {
        protected JMySqlDbContext() {
        }
        
        protected JMySqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL(DbConnectionProvider.MYSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    /// <summary>
    ///     sqlite dbcontext
    /// </summary>
    public class JSqlLiteDbContext : JEFDbContextBase {
        protected JSqlLiteDbContext() {
        }
        
        protected JSqlLiteDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(DbConnectionProvider.SQLITE);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    /// <summary>
    ///     postgres sql dbcontext
    /// </summary>
    public class JNpgSqlDbContext : JEFDbContextBase {
        protected JNpgSqlDbContext() {
        }        
        protected JNpgSqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(DbConnectionProvider.NPGSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class DbConnectionProvider {
        public static readonly string MSSQL = "[connection string]".toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public static readonly string MYSQL = "[connection string]".toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public static readonly string SQLITE = "[connection string]".toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);

        public static readonly string NPGSQL = "[connection string]".toDecAes256(DbCipherKeyIVProvider.Instance.Key,
            DbCipherKeyIVProvider.Instance.IV, CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
    }

    public class DbCipherKeyIVProvider {
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
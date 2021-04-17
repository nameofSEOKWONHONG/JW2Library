using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    /// <summary>
    ///     sqlite dbcontext
    /// </summary>
    public class JSqlLiteDbContext : JEfDbContextBase {
        protected JSqlLiteDbContext() {
        }
        
        protected JSqlLiteDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(DbConnectionProvider.Instance.SQLITE);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class JSqlLiteInMeoryDbContext : JEfDbContextBase {
        public JSqlLiteInMeoryDbContext() {
            
        }

        public JSqlLiteInMeoryDbContext(DbContextOptions options) : base(options) {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(DbConnectionProvider.Instance.SQLITE_IN_MEMORY);
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    /// <summary>
    ///     postgres sql dbcontext
    /// </summary>
    public class JNpgSqlDbContext : JEfDbContextBase {
        protected JNpgSqlDbContext() {
        }        
        protected JNpgSqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(DbConnectionProvider.Instance.NPGSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
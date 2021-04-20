using JWLibrary.Database;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    /// <summary>
    ///     mysql dbcontext
    /// </summary>
    public class JMySqlDbContext : JEfDbContextBase {
        protected JMySqlDbContext() {
        }
        
        protected JMySqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL(DbConnectionProvider.Instance.MYSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
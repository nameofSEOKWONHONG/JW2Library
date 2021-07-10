using JWLibrary.Database;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF
{
    /// <summary>
    ///     mssql dbcontext
    /// </summary>
    public class JSqlDbContext : JEfDbContextBase
    {
        protected JSqlDbContext()
        {
        }

        protected JSqlDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnectionProvider.Instance.MSSQL);
            //base.OnConfiguring(optionsBuilder);
        }
    }
}
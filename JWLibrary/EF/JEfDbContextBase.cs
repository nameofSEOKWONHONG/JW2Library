using eXtensionSharp;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF
{
    /// <summary>
    ///     base db context
    /// </summary>
    public abstract class JEfDbContextBase : DbContext
    {
        protected static bool IsCreated;

        protected JEfDbContextBase()
        {
            if (IsCreated.xIsFalse())
            {
                IsCreated = true;
                Database.Migrate();
                Database.EnsureCreated();
            }
        }

        protected JEfDbContextBase(DbContextOptions options) : base(options)
        {
        }
    }
}
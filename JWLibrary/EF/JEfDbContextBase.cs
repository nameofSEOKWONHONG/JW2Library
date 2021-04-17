using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    /// <summary>
    ///     base db context
    /// </summary>
    public abstract class JEfDbContextBase : DbContext {
        protected static bool IsCreated = false;
        protected JEfDbContextBase() {
            if (IsCreated.isFalse()) {
                IsCreated = true;
                this.Database.Migrate();
                this.Database.EnsureCreated();
            }
        }

        protected JEfDbContextBase(DbContextOptions options) : base(options) {
        }
    }
}
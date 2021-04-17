using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Resources;
using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using RepoDb.Attributes;

namespace JWLibrary.EF {
    /// <summary>
    /// Entity
    /// </summary>
    public class Blog {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required()]
        public string BlogName { get; set; }
        [MaxLength(30)]
        [Required()]
        public string BlogAuthor { get; set; }
    }
    
    /// <summary>
    /// mssql blog dbcontext
    /// </summary>
    public class BlogSqlContext : JSqlDbContext {
        public DbSet<Blog> Blogs { get; set; }

        public BlogSqlContext() {
            if (IsCreated.isFalse()) {
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
        }
        
        public BlogSqlContext(DbContextOptions<BlogSqlContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Blog>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();
            //base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// sqlite blog dbcontext
    /// </summary>
    public class BlogSqliteDbContext : JSqlLiteDbContext {
        public DbSet<Blog> Blogs { get; set; }
        
        public BlogSqliteDbContext() {
            if (IsCreated.isFalse()) {
                IsCreated = true;
                this.Database.Migrate();
                this.Database.EnsureCreated();
            }
        }

        public BlogSqliteDbContext(DbContextOptions<BlogSqliteDbContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Blog>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();
            //base.OnModelCreating(modelBuilder);
        }
    }
}
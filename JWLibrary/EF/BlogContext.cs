using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Resources;
using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations;
using NetFabric.Hyperlinq;
using RepoDb.Attributes;

namespace JWLibrary.EF {
    /// <summary>
    /// Entity
    /// </summary>
    public class Blog {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required()]
        public string BlogName { get; set; }
        [MaxLength(30)]
        [Required()]
        public string BlogAuthor { get; set; }
        
        [Required()]
        public DateTime WriteDate { get; set; }

        public class BlogMigration : Migration {
            protected override void Up(MigrationBuilder migrationBuilder) {
                migrationBuilder.AddColumn<string>("WriteDate", "Blogs", "DATETIME", unicode:false);
            }
        }
    }
    
    /// <summary>
    /// mssql blog dbcontext
    /// </summary>
    public class BlogSqlContext : JSqlDbContext {
        public DbSet<Blog> Blogs { get; set; }

        public BlogSqlContext() {

        }
        
        public BlogSqlContext(DbContextOptions<BlogSqlContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Blog>();
            //base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// sqlite blog dbcontext
    /// </summary>
    public class BlogSqliteDbContext : JSqlLiteDbContext {
        public DbSet<Blog> Blogs { get; set; }
        
        public BlogSqliteDbContext() {
            
        }

        public BlogSqliteDbContext(DbContextOptions<BlogSqliteDbContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
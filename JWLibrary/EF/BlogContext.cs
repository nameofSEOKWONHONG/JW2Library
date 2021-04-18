using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JWLibrary.EF {
    public interface IMigration {
        void IfExistsTable(Func<string, string> func);
        void CreateTable(Func<string, string> func);
        void BackupTable(Func<string, string> func);
        void IfExistsCreateTable(Func<string, string> func);
    }
    
    public abstract class JMigration : IMigration {
        public abstract void IfExistsTable(Func<string, string> func);

        public abstract void CreateTable(Func<string, string> func);

        public abstract void BackupTable(Func<string, string> func);

        public abstract void IfExistsCreateTable(Func<string, string> func);
    }
    
    /// <summary>
    /// Entity
    /// </summary>
    [Table("Blogs")]
    public class Blog {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [MaxLength(100)]
        [Required()]
        public string BLOG_NAME { get; set; }
        [MaxLength(30)]
        [Required()]
        public string BLOG_AUTHOR { get; set; }
        
        [Required()]
        public DateTime WRITE_DT { get; set; }
        
        public class BlogMigration : JMigration {
            public override void IfExistsTable(Func<string, string> func) {
                func(@"
IF (EXISTS (SELECT * 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'DBO' 
            AND  TABLE_NAME = 'BLOGS'))
BEGIN
    SELECT TRUE;
END
ELSE
BEGIN
    SELECT FALSE;
END
");
            }

            public override void CreateTable(Func<string, string> func) {
                func(@"
CREATE TABLE [DBO].[BLOGS] (
    ID INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    BLOG_NAME VARCHAR(100) NOT NULL,
    BLOG_AUTHOR VARCHAR(30) NOT NULL,
    WRITE_DT DATETIME NOT NULL
)
");
                
            }

            public override void BackupTable(Func<string, string> func) {
                func(@"
DROP TABLE IF EXISTS BLOG_TEMP

SELECT *
INTO BLOG_TEMP
FROM ACCT.DBO.BLOGS
");
            }

            public override void IfExistsCreateTable(Func<string, string> func) {
                func(@"
ALTER TABLE...
");
            }
        }

        // public class BlogMigration : Migration {
        //     protected override void Up(MigrationBuilder migrationBuilder) {
        //         migrationBuilder.CreateTable(
        //             name: "Blogs",
        //             columns: table => new {
        //                 Id = table.Column<int>(nullable: false),
        //                 BlogName = table.Column<string>(nullable: false),
        //                 BlogAuthor = table.Column<string>(nullable: false),
        //             },
        //             constraints: table => {
        //                 table.PrimaryKey("PK_Blogs", x => x.Id);
        //             });
        //         migrationBuilder.AddColumn<DateTime>("WriteDate", "Blogs", unicode:false);
        //     }
        // }
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
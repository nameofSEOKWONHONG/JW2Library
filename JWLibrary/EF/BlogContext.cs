using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Dapper;
using JWLibrary.Database;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Attributes;

namespace JWLibrary.EF
{
    /// <summary>
    ///     Entity
    /// </summary>
    [Table("Blogs")]
    [BsonIgnoreExtraElements]
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(100)] [Required] public string BLOG_NAME { get; set; }

        [MaxLength(30)] [Required] public string BLOG_AUTHOR { get; set; }

        [Required] public DateTime WRITE_DT { get; set; }

        public class BlogMSSqlMigration : JMigration
        {
            public override bool IsExistsTable(IDbConnection connection)
            {
                var sql = @"
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
";
                return connection.QueryFirst<bool>(sql);
            }

            public override void CreateTable(IDbConnection connection)
            {
                var sql = @"
CREATE TABLE [DBO].[BLOGS] (
    ID INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    BLOG_NAME VARCHAR(100) NOT NULL,
    BLOG_AUTHOR VARCHAR(30) NOT NULL,
    WRITE_DT DATETIME NOT NULL
)
";
                connection.Execute(sql);
            }

            public override void CreateTempTable(IDbConnection connection)
            {
                var sql = @"
DROP TABLE IF EXISTS BLOG_TEMP

SELECT *
INTO BLOG_TEMP
FROM ACC.DBO.BLOGS
";
                connection.Execute(sql);
            }

            public override void AfterProcess(IDbConnection connection)
            {
                var sql = @"
CREATE TABLE [DBO].[BLOGS] (
    ID INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    BLOG_NAME VARCHAR(100) NOT NULL,
    BLOG_AUTHOR VARCHAR(30) NOT NULL,
    WRITE_DT DATETIME NOT NULL
)

SET IDENTITY_INSERT ACC.DBO.BLOG OFF

INSERT INTO ACC.DBO.BLOG
SELECT *
FROM ACC.DBO.BLOG_TEMP

SET IDENTITY_INSERT ACC.DBO.BLOG ON
";
                connection.Execute(sql);
            }
        }

        public class BlogMySqlMigration : JMigration
        {
            public override bool IsExistsTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTempTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void AfterProcess(IDbConnection connection)
            {
                throw new NotImplementedException();
            }
        }

        public class BlogNpgSqlMigration : JMigration
        {
            public override bool IsExistsTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTempTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void AfterProcess(IDbConnection connection)
            {
                throw new NotImplementedException();
            }
        }

        public class BlogSqliteMigration : JMigration
        {
            public override bool IsExistsTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void CreateTempTable(IDbConnection connection)
            {
                throw new NotImplementedException();
            }

            public override void AfterProcess(IDbConnection connection)
            {
                throw new NotImplementedException();
            }
        }

        #region [no use - ef core migration builder]

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
        //         migrationBuilder.
        //     }
        // }

        #endregion
    }

    public class BlogDetail
    {
        public int ID { get; set; }

        public int BLOG_ID { get; set; }

        public string CONTENTS { get; set; }
    }

    /// <summary>
    ///     mssql blog dbcontext
    /// </summary>
    public class BlogSqlContext : JSqlDbContext
    {
        public BlogSqlContext()
        {
        }

        public BlogSqlContext(DbContextOptions<BlogSqlContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>();
            //base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    ///     sqlite blog dbcontext
    /// </summary>
    public class BlogSqliteDbContext : JSqlLiteDbContext
    {
        public BlogSqliteDbContext()
        {
        }

        public BlogSqliteDbContext(DbContextOptions<BlogSqliteDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}
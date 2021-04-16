using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Npgsql;

namespace JWLibrary.Database {
    public class JDataBase {
        private static readonly Lazy<JDatabaseInfo> JDataBaseInfo = new(() => { return new JDatabaseInfo(); });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection))
                return JDataBaseInfo.Value.Connections["MSSQL"];
            if (typeof(TDatabase) == typeof(MySqlConnection))
                return JDataBaseInfo.Value.Connections["MYSQL"];
            if (typeof(TDatabase) == typeof(NpgsqlConnection))
                return JDataBaseInfo.Value.Connections["NPGSQL"];
            throw new NotImplementedException();
        }

        public static (IDbConnection, IDbConnection) Resolve<TDatabaseA, TDatabaseB>()
            where TDatabaseA : IDbConnection
            where TDatabaseB : IDbConnection {
            if (typeof(TDatabaseA) == typeof(TDatabaseB)) throw new Exception("not allow same database connection.");
            return new ValueTuple<IDbConnection, IDbConnection>(Resolve<TDatabaseA>(), Resolve<TDatabaseB>());
        }
    }

    #region [ef core base class]

    public abstract class JEfDbContext : DbContext {
        protected JEfDbContext(string connection) {
            Connection = connection;
        }

        protected JEfDbContext(DbContextOptions options) : base(options) {
        }

        protected string Connection { get; }
    }

    public class JSqlDbContext : JEfDbContext {
        protected JSqlDbContext(string connection)
            : base(connection) {
        }

        protected JSqlDbContext(DbContextOptions options)
            : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(Connection);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class JMySqlDbContext : JEfDbContext {
        protected JMySqlDbContext(string connection) : base(connection) {
        }

        protected JMySqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySQL(Connection);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class JSqlLiteDbContext : JEfDbContext {
        public JSqlLiteDbContext(string connection) : base(connection) {
        }

        public JSqlLiteDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(this.Connection);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    public class JNpgSqlDbContext : JEfDbContext {
        public JNpgSqlDbContext(string connection) : base(connection) {
        }

        public JNpgSqlDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(this.Connection);
            //base.OnConfiguring(optionsBuilder);
        }
    }

    #endregion

    #region [test code]

    // public class BlogContext : JSqlDbContext {
    //     public DbSet<Blog> Blogs { get; set; }
    //     public class Blog {
    //         public int Id { get; set; }
    //         public string BlogName { get; set; }
    //         public string BlogAuthor { get; set; }
    //     }
    //
    //     public BlogContext(string connection) : base(connection) {
    //     }
    //
    //     public BlogContext(DbContextOptions<BlogContext> options) : base(options) {
    //     }
    // }

    // public class T2 {
    //     public void Run() {
    //         var contextOptions = new DbContextOptionsBuilder<BlogContext>()
    //             .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test")
    //             .Options;
    //         using (var context = new BlogContext(contextOptions)) {
    //             var selectedblog = context.Blogs.FirstOrDefault(m => m.Id.Equals(1));
    //             if (selectedblog.isNotNull()) {
    //                 selectedblog.BlogName = "test";
    //                 selectedblog.BlogAuthor = "test";
    //                 context.Blogs.Update(selectedblog);
    //                 context.SaveChanges();
    //                 context.Blogs.AsQueryable().Expression.ToString();
    //             }
    //         }
    //     }
    // }

    #endregion
}
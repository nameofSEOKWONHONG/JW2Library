using System;
using System.IO;
using System.Linq;
using eXtensionSharp;
using JWLibrary.EF;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class EfContextTest {
        private BlogSqlContext _context;
        private BlogSqliteDbContext _sqliteDbContext;

        [SetUp]
        public void Setup() {
            _context = new BlogSqlContext();
            _sqliteDbContext = new BlogSqliteDbContext();
        }

        #region [dbcontext test]

        [Test]
        public void blog_dbcontext_test() {
            try {
                _context.Blogs.Add(new Blog {
                    BLOG_NAME = "test1",
                    BLOG_AUTHOR = "test1",
                    WRITE_DT = DateTime.Now
                });

                _context.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            finally {
                _context.Dispose();
            }
        }

        [Test]
        public void blog_dbcontext_get_test() {
            try {
                var exists = _context.Blogs.First(m => m.BLOG_NAME == "test1");
                if (exists.xIsNotNull())
                    Assert.Pass();
                else
                    Assert.Fail();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                _context.Dispose();
            }
        }

        [Test]
        public void blog_sqlitedbcontext_test() {
            try {
                var srcExists = _context.Blogs.FirstOrDefault(m => m.BLOG_NAME == "test1");
                if (srcExists.xIsNotNull()) {
                    _sqliteDbContext.Blogs.Add(srcExists);
                    var i = _sqliteDbContext.SaveChanges();
                    Assert.Greater(i, 0);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                _sqliteDbContext.Dispose();
            }
        }

        [Test]
        public void blog_sqlitedbcontext_get_test() {
            try {
                var exists = _sqliteDbContext.Blogs.First(m => m.BLOG_NAME == "test1");
                if (exists.xIsNotNull()) Assert.Pass();
                else Assert.Fail();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        [Test]
        public void migration_test() {
            // var executor = new MigrationExecutor(new[]{Path.Combine("".xToPath("bin\\debug\\net5.0"), "JWLibrary.dll")});
            // executor.Execute();
        }

        [Test]
        public void query_expression_test() {
        }
    }
}
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Transactions;
using Community.CsharpSqlite;
using JWLibrary.Core;
using JWLibrary.EF;
using Microsoft.EntityFrameworkCore;
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
                _context.Blogs.Add(new Blog() {
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
                if (exists.jIsNotNull())
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
                if (srcExists.jIsNotNull()) {
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
                var exists =_sqliteDbContext.Blogs.First(m => m.BLOG_NAME == "test1");
                if(exists.jIsNotNull()) Assert.Pass();
                else Assert.Fail();

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                
            }
        }        

        #endregion

        #region [dbcontext sync test]

        [Test]
        public void blog_sync_test() {
            IJDbSyncContext context = new JDbSyncContext(
                srcDbContext:new BlogSqlContext(), 
                destDbContext:new JList<DbContext>() {
                    new BlogSqliteDbContext(),
                });
            
            context.Insert<Blog>(db => {
                Blog blog = new Blog() {
                    BLOG_NAME = "test2",
                    BLOG_AUTHOR = "test2",
                    WRITE_DT = DateTime.Now
                };
                return blog;
            });
            
            context.Update<Blog>(db => {
                var dbcontext = db as BlogSqlContext;
                var exists = dbcontext.Blogs.FirstOrDefault();
                exists.BLOG_AUTHOR = exists.BLOG_AUTHOR + "!@!@";
                return exists;
            });
            
            context.Delete<Blog>(db => {
                var dbcontext = db as BlogSqlContext; 
                return dbcontext.Blogs.LastOrDefault();
            });
            
            context.Dispose();
        }

        #endregion

        [Test]
        public async Task dbcontext_sync_async_test() {
            IJDbSyncContext context = new JDbSyncContext(
                srcDbContext:new BlogSqlContext(), 
                destDbContext:new JList<DbContext>() {
                    new BlogSqliteDbContext(),
                });
            
            context.InsertAsync<Blog>(db => {
                Blog blog = new Blog() {
                    BLOG_NAME = "test2",
                    BLOG_AUTHOR = "test2",
                    WRITE_DT = DateTime.Now
                };
                return blog;
            });
            
            context.UpdateAsync<Blog>(db => {
                var dbcontext = db as BlogSqlContext;
                var exists = dbcontext.Blogs.FirstOrDefault();
                exists.BLOG_AUTHOR = exists.BLOG_AUTHOR + "!@!@";
                return exists;
            });
            //
            // context.DeleteAsync<Blog>(db => {
            //     var dbcontext = db as BlogSqlContext; 
            //     return dbcontext.Blogs.LastOrDefault();
            // });
            //
            context.Dispose();
        }

        [Test]
        public void sync_transaction_test() {
            using var syncContext = new JDbSyncContext(new BlogSqlContext(), new[] {new BlogSqliteDbContext()});
            try {
                syncContext.Insert(db => {
                    return new Blog() {
                        BLOG_NAME = "test3",
                        BLOG_AUTHOR = "test3",
                        WRITE_DT = DateTime.Now
                    };
                });

                //throw new Exception();
            
                syncContext.Update(db => {
                    var dbcontext = db as BlogSqlContext;
                    var exists = dbcontext.Blogs.FirstOrDefault(m => m.BLOG_NAME == "test3");
                    if (exists.jIsNotNull()) {
                        exists.BLOG_NAME = "test33";
                        exists.BLOG_AUTHOR = "test33";
                    }

                    return exists;
                });
            }
            catch (Exception e) {
                syncContext.DoRollback();
            }
        }
    }
}
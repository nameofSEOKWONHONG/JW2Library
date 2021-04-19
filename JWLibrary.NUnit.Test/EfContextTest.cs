using System;
using System.Linq;
using System.Net.Mail;
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
                _sqliteDbContext.Blogs.Add(new Blog() {
                    BLOG_NAME = "test1",
                    BLOG_AUTHOR = "test1",
                    WRITE_DT = DateTime.Now
                });
                var i = _sqliteDbContext.SaveChanges();
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
    }
}
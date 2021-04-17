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
                    BlogName = "test1",
                    BlogAuthor = "test1"
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
                var exists = _context.Blogs.First(m => m.BlogName == "test1");
                if (exists.isNotNull())
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
                    BlogName = "test1",
                    BlogAuthor = "test1"
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
                var exists =_sqliteDbContext.Blogs.First(m => m.BlogName == "test1");
                if(exists.isNotNull()) Assert.Pass();
                else Assert.Fail();

            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            finally {
                
            }
        }
    }
}
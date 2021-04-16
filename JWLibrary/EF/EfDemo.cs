using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using JWLibrary.Core;
using Microsoft.EntityFrameworkCore;

namespace JWLibrary.EF {
    public class BlogContext : JSqlDbContext {
        public DbSet<Blog> Blogs { get; set; }
        public class Blog {
            [Key]
            public int Id { get; set; }
            [MaxLength(100)]
            public string BlogName { get; set; }
            [MaxLength(30)]
            public string BlogAuthor { get; set; }
        }

        public BlogContext() {
            
        }
        
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {
        }
    }

    public class EfDemo {
        public void Run() {
            var contextOptions = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test")
                .Options;

            using (var context = new BlogContext()) {
                context.Blogs.First(m => m.Id.isEquals<int>(1));
            }
            
            using (var context = new BlogContext(contextOptions)) {
                var selectedblog = context.Blogs.FirstOrDefault(m => m.Id.Equals(1));
                if (selectedblog.isNotNull()) {
                    selectedblog.BlogName = "test";
                    selectedblog.BlogAuthor = "test";
                    context.Blogs.Update(selectedblog);
                    context.SaveChanges();
                    context.Blogs.AsQueryable().Expression.ToString();
                }
            }
        }
    }
}
using System;
using JWLibrary.Database;
using JWLibrary.EF;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class SqlExpressionTest {
        [Test]
        public void select_query_test() {
            var sqlExpression = new SqlExpression();
            var sql = sqlExpression.Select<Blog>(m => new {m.BLOG_NAME, m.BLOG_AUTHOR})
                .Where(m => m.ID == 1)
                .And(m => m.BLOG_NAME == "test")
                .Build();
            
            Console.WriteLine(sql);
        }

        [Test]
        public void select_all_test() {
            var sqlExpression = new SqlExpression();
            var sql = sqlExpression.Select<Blog>();
            Console.WriteLine(sql.Build());
        }

        [Test]
        public void select_join_test() {
            var sqlExpression = new SqlExpression();
            var sql = sqlExpression.Join<Blog, BlogDetail>((blog, detail) => new {
                ID = blog.ID,
                BLOG_NAME = blog.BLOG_NAME,
                BLOG_AUTHOR = blog.BLOG_AUTHOR,
                WRITE_DT = blog.WRITE_DT,
                CONTENTS = detail.CONTENTS
            });
        }
    }
}
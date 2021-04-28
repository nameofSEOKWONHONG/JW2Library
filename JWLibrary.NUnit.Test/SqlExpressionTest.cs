using System;
using JWLibrary.Database;
using JWLibrary.EF;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class SqlExpressionTest {
        [Test]
        public void select_query_test() {
            var srcSql = @"SELECT BLOG_NAME, BLOG_AUTHOR
FROM Blog
WHERE ID = 1
AND BLOG_NAME = 'test'
AND BLOG_NAME LIKE ('test')
AND ID IN (1, 2, 3, 4)
";
            var sqlExpression = new JSqlExpression();
            var sql = sqlExpression.Select<Blog>(m => new {m.BLOG_NAME, m.BLOG_AUTHOR})
                .Where(m => m.ID == 1)
                .And(m => m.BLOG_NAME == "test")
                .And(m => Sql.Like(m.BLOG_NAME, "test"))
                .And(m => Sql.In(m.ID, new[]{1,2,3,4}))
                .Build();
            
            Assert.AreEqual(srcSql, sql);
        }

        [Test]
        public void select_all_test() {
            var srcSql = @"SELECT ID, BLOG_NAME, BLOG_AUTHOR, WRITE_DT
FROM Blog
";
            var sqlExpression = new JSqlExpression();
            var sql = sqlExpression.Select<Blog>().Build();
            
            Assert.AreEqual(srcSql, sql);
        }

        [Test]
        public void select_join_test() {
            var srcSql = @"SELECT blog.ID, blog.BLOG_NAME, blog.BLOG_AUTHOR, blog.WRITE_DT, detail.CONTENTS
FROM Blog blog JOIN BlogDetail detail
ON blog.ID=detail.BLOG_ID
AND blog.BLOG_NAME=detail.CONTENTS
WHERE blog.ID=1
AND detail.ID=1
AND CONTENTS LIKE ('test')
AND ID IN (1, 2, 3, 4)
";
            
            var sqlExpression = new JSqlExpression();
            var sql = sqlExpression.Join<Blog, BlogDetail>((blog, detail) => new {
                    ID = blog.ID,
                    BLOG_NAME = blog.BLOG_NAME,
                    BLOG_AUTHOR = blog.BLOG_AUTHOR,
                    WRITE_DT = blog.WRITE_DT,
                    CONTENTS = detail.CONTENTS
                }).On((blog, detail) => blog.ID == detail.BLOG_ID)
                .OnAnd((blog, detail) => blog.BLOG_NAME == detail.CONTENTS)
                .Where((blog, detail) => blog.ID == 1)
                .And((blog, detail) => detail.ID == 1)
                .And((blog, detail) => Sql.Like(detail.CONTENTS, "test"))
                .And((blog, detail) => Sql.In(detail.ID, new[] {1, 2,3 ,4}))
                .Build();
            
            Assert.AreEqual(srcSql, sql);
        }
    }
}
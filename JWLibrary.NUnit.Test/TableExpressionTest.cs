using System.ComponentModel.Design.Serialization;
using JWLibrary.Database;
using JWLibrary.EF;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class TableExpressionTest {
        //TODO:구현해야함.
        [Test]
        public void CreateTableTest() {
            var expectedSql = @"";
            var tableExpression = new JTableExpression<BlogDetail>();
            var createTable = tableExpression.PK(m => new {m.ID})
                .AutoIncrementKey(m => m.ID)
                .FK<Blog>((detail, blog) => new {detail.BLOG_ID, blog.ID})
                .Index(m => new {m.ID, m.BLOG_ID, m.CONTENTS})
                .Build();
            
            Assert.AreEqual(expectedSql, createTable);
        }
    }
}
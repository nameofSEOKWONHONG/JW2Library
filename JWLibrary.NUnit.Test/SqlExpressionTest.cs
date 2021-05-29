using System;
using System.Collections.Generic;
using Community.CsharpSqlite;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.EF;
using NUnit.Framework;
using TodoService.Data;

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

        [Test]
        public void migration_expression_test() {
            var em = new EntityMigration<USER>();
            var sql = em
                .Key("ID", "INT", () => "IDENTITY(1, 1)")
                .Column("USER_ID", "VARCHAR(30)", () => "NOT NULL")
                .Column("PASSWORD", "VARCHAR(20)", () => "NOT NULL")
                .Column("USER_NM", "VARCHAR(30)", () => "NOT NULL")
                .Column("NICK_NM", "VARCHAR(30)", () => "NOT NULL")
                .Column("REG_DT", "DATETIME", () => "NOT NULL")
                .Column("IS_EXFIRED", "BIT", () => "DEFAULT FALSE")
                .Build();
            
            Console.WriteLine(sql);
            
            /*
             * DROP TABLE IF EXISTS DBO.USER
             * CREATE TABLE DBO.USER
             * (
             * ID INT PRIMARY KEY IDENTITY(1,1),
             * USER_ID VARCHAR(10) NOT NULL,
             * NICK_NM VARCHAR(10) NOT NULL
             * )
             */
        }
    }

    public class EntityMigration<TEntity> where TEntity : class {
        private string _tableName;
        private string _createExpression;
        private string _dropExpression;
        private string _backupExpression;
        private string _backupInsertExpression;
        private List<string> _keyExpressions = new List<string>();
        private List<string> _columnExpressions = new List<string>();
        
        public EntityMigration(bool exists = false) {
            _tableName = typeof(TEntity).Name;
            if (exists) {
                _backupExpression = $"SELECT * INTO {_tableName}_BACKUP FROM {_tableName}";
            }

            _dropExpression = $"DROP TABLE IF EXISTS DBO.{_tableName}";
            _createExpression = $"CREATE TABLE DBO.{_tableName}";

            if (exists) {
                _backupInsertExpression = $"INSERT INTO DBO.USER {_tableName} SELECT * FROM DBO.{_tableName}";    
            }
        }
        
        public EntityMigration<TEntity> Key(string key, string type, bool exists = false) {
            var keyExpression = $"{key.ToUpper()} {type.ToUpper()} PRIMARY KEY";
            _keyExpressions.Add(keyExpression);
            return this;
        }
        
        public EntityMigration<TEntity> Key(string key, string type, Func<string> option = null, bool exists = false) {
            var keyExpression = $"{key.ToUpper()} {type.ToUpper()} PRIMARY KEY {option().ToUpper()}";
            _keyExpressions.Add(keyExpression);
            return this;
        }

        public EntityMigration<TEntity> Column(string column, string type, bool exists = false) {
            var columnExpression = $"{column.ToUpper()} {type.ToUpper()}";
            _columnExpressions.Add(columnExpression);
            return this;
        }
        
        public EntityMigration<TEntity> Column(string column, string type, Func<string> option = null, bool exists = false) {
            var columnExpression = $"{column.ToUpper()} {type.ToUpper()} {option().ToUpper()}";
            _columnExpressions.Add(columnExpression);
            return this;
        }

        public string Build() {
            var sb = new XStringBuilder();
            sb.AppendLine(_backupExpression);
            sb.AppendLine(_dropExpression);
            sb.AppendLine(_createExpression);
            sb.AppendLine("(");
            _keyExpressions.xForEach(item => {
                sb.AppendLine($"{item},");
            });
            
            _columnExpressions.xForEach((item, i) => {
                if (i == _columnExpressions.Count - 1) {
                    sb.AppendLine($"{item}");
                }
                else {
                    sb.AppendLine($"{item}, ");
                }
            });
            sb.AppendLine(")");
            var query = string.Empty;
            sb.Release(out query);
            return query;
        }
    }
}
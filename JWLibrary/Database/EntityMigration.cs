using System;
using System.Collections.Generic;
using eXtensionSharp;
using Microsoft.Data.SqlClient;
using RepoDb;

namespace JWLibrary.Database {
    /*
     * note : 엔티티 마이그레이션의 구현은 sqlkata에서 영감을 받아 구현함.
     * date : 2021.05.29
     * writer : 홍석원
     */
    
    /// <summary>
    /// mssql, mysql, postgresql
    /// 엔티티 마이그레이션 인터페이스
    /// 인터페이스이므로 다구현을 예측해야 한다.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityMigration<TEntity> where TEntity : class {
        EntityMigration<TEntity> Key(string key, string type, Func<string> option = null);
        EntityMigration<TEntity> Column(string column, string type, Func<string> option = null);
        string Build();
    }

    /// <summary>
    /// mysql 엔티티 마이그레이션 구현
    /// TODO : mysql 구현해 보자
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MySqlEntityMigration<TEntity> : IEntityMigration<TEntity> where TEntity : class {
        public EntityMigration<TEntity> Key(string key, string type, Func<string> option = null) {
            throw new NotImplementedException();
        }

        public EntityMigration<TEntity> Column(string column, string type, Func<string> option = null) {
            throw new NotImplementedException();
        }

        public string Build() {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// postgresql 엔티티 마이그레이션 구현
    /// TODO : 구현해야함.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PostgreSqlEntityMigration<TEntity> : IEntityMigration<TEntity> where TEntity : class {
        public EntityMigration<TEntity> Key(string key, string type, Func<string> option = null) {
            throw new NotImplementedException();
        }

        public EntityMigration<TEntity> Column(string column, string type, Func<string> option = null) {
            throw new NotImplementedException();
        }

        public string Build() {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// mssql 엔티티 마이그레이션 구현
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityMigration<TEntity> : IEntityMigration<TEntity>
        where TEntity : class {
        private string _tableName;
        private string _createExpression;
        private string _dropExpression;
        private string _backupExpression;
        private string _backupInsertExpression;
        private List<string> _columnExpressions = new List<string>();
        
        public EntityMigration() {
            _tableName = typeof(TEntity).Name;
        }
        
        public EntityMigration<TEntity> Key(string key, string type, Func<string> option = null) {
            var keyExpression = string.Empty;
            if (option.xIsNotNull()) {
                keyExpression = $"{key.ToUpper()} {type.ToUpper()} PRIMARY KEY {option().ToUpper()}";
            }
            else {
                keyExpression = $"{key.ToUpper()} {type.ToUpper()} PRIMARY KEY";
            }
            _columnExpressions.Add(keyExpression);
            return this;
        }
        public EntityMigration<TEntity> Column(string column, string type, Func<string> option = null) {
            var columnExpression = string.Empty;
            if (option.xIsNotNull()) {
                columnExpression = $"{column.ToUpper()} {type.ToUpper()} {option().ToUpper()}";
            }
            else {
                columnExpression = $"{column.ToUpper()} {type.ToUpper()}";
            }
            _columnExpressions.Add(columnExpression);
            return this;
        }

        public string Build() {
            IEnumerable<string> existsTableColumns = null;

            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecute((db, tran) => {
                    var sql = "SELECT B.COLUMN_NAME " +
                              "FROM INFORMATION_SCHEMA.TABLES A JOIN INFORMATION_SCHEMA.COLUMNS B " +
                              "ON A.TABLE_SCHEMA = B.TABLE_SCHEMA AND A.TABLE_NAME = B.TABLE_NAME " +
                              "WHERE A.TABLE_NAME = @TABLE_NAME ";
                    existsTableColumns = db.ExecuteQuery<string>(sql, new {TABLE_NAME = _tableName}).xToList();
                });

            if (existsTableColumns.xIsNotEmpty()) {
                return Scenario1(existsTableColumns);
            }

            return CreateTable();
        }
        
        /// <summary>
        /// SELECT *
        /// INTO USER_BACKUP
        /// FROM USER;
        /// 
        /// DROP TABLE IF EXISTS DBO.USER
        /// CREATE TABLE DBO.USER
        /// (
        /// ID INT PRIMARY KEY IDENTITY(1,1),
        /// USER_ID VARCHAR(10) NOT NULL,
        /// NICK_NM VARCHAR(10) NOT NULL
        /// )
        /// 
        /// INSERT INTO USER (ID, USER_ID, NICK_NM) SELECT ID, USER_ID, NICK_NM FROM USER_BACKUP
        /// </summary>
        /// <param name="existsColumns"></param>
        /// <returns></returns>
        private string Scenario1(IEnumerable<string> existsColumns) {
            _backupExpression = $"SELECT {string.Join(",", existsColumns)} INTO {_tableName}_BACKUP FROM [{_tableName}]";
            _dropExpression = $"DROP TABLE IF EXISTS [{_tableName}]";
            _createExpression = CreateTable();
            _backupInsertExpression = $"INSERT INTO [{_tableName}] ({string.Join(",",existsColumns)}) SELECT {string.Join(",",existsColumns)} FROM [{_tableName}_BACKUP]";

            var sb = new XStringBuilder();
            sb.AppendLine(_backupExpression);
            sb.AppendLine(_dropExpression);
            sb.AppendLine(_createExpression);
            sb.AppendLine(_backupInsertExpression);
            var query = string.Empty;
            sb.Release(out query);
            return query;
        }

        private string CreateTable() {
            var sb = new XStringBuilder();
            sb.AppendLine($"CREATE TABLE [{_tableName}]");
            sb.AppendLine("(");
            
            _columnExpressions.xForEach(",", (item, split) => {
                sb.AppendLine($"{item}{split}");
            });
            
            sb.AppendLine(")");
            var query = string.Empty;
            sb.Release(out query);
            return query;
        }
    }
}
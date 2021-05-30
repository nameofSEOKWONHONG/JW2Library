using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using eXtensionSharp;
using MongoDB.Bson.IO;
using MySql.Data.MySqlClient;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JWLibrary.Database {
    /// <summary>
    ///     Database Client Extension
    /// TODO:Transaction Commit 부분 변경해야 함.
    /// </summary>
    public class JDbClientBulkExtension {
        /// <summary>
        ///     bulk insert, use datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="bulkDatas"></param>
        /// <param name="tableName"></param>
        public void BulkInsert<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new() {
            try {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
            finally {
                connection.Close();
            }
        }

        public async Task BulkInsertAsync<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new() {
            try {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    await bulkCopy.WriteToServerAsync(dt);
                }
            }
            finally {
                connection.Close();
            }
        }
    }

    /// <summary>
    ///     create dbconnection only
    /// </summary>
    public class JDbClientExecutor : IDisposable {
        private bool _isThrow;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private Func<IDbConnection, IDbTransaction> _addTran;

        public JDbClientExecutor(IDbConnection connection) {
            _connection = connection;
        }

        public JDbClientExecutor BeginTran() {
            _addTran = connection => connection.BeginTransaction(); 
            return this;
        }
        
        #region [self impletment func method]

        /// <summary>
        ///     you can execute func method, use dbconnection, any code. (use dapper, ef, and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public JDbClientExecutor DbExecute(Action<IDbConnection, IDbTransaction> action) {
            try {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                action(_connection, _transaction);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }

        public JDbClientExecutor DbExecuteKata(Action<IDbConnection, QueryFactory> action) {
            try {
                _connection.xOpen();

                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                
                var queryFactory = SqlKataCompilerFactory.CreateInstance(_connection);
                if (queryFactory.xIsNull()) throw new NotImplementedException();

                action(_connection, queryFactory);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }

        /// <summary>
        ///     async execute(select, update, delete, insert) db, use any code on func method. (use dapper, ef and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<JDbClientExecutor> DbExecutorAsync(Func<IDbConnection, Task> func) {
            try {
                _connection.xOpen();
                if (_addTran.xIsNotNull()) {
                    _transaction = _addTran(_connection);    
                }
                await func(_connection);
            }
            catch {
                if(_transaction.xIsNotNull()) _transaction.Rollback();
                _transaction = null;
                throw;
            }

            return this;
        }
        
        #endregion [self impletment func method]
        public void Dispose() {
            if (_transaction.xIsNotNull()) {
                _transaction.Commit();
            }
            if (_connection.xIsNotNull()) {
                _connection.xClose();
            }
        }
    }

    public static class JDbExtension {
        public static void xOpen(this IDbConnection connection) {
            if(connection.State != ConnectionState.Open) connection.Open();
        }

        public static void xClose(this IDbConnection connection) {
            if(connection.State == ConnectionState.Open) connection.Close();
        }
    }

    // TODO : 쓸것인가 말것인가... using 안쓸려면 이걸로 해야 하는데...
    // public class JTransaction {
    //     private IDbConnection _connection;
    //     private ConcurrentStack<IDbTransaction>
    //         _transactions = new ConcurrentStack<IDbTransaction>();
    //
    //     public int Count {
    //         get {
    //             return _transactions.Count();
    //         }
    //     }
    //
    //     public JTransaction(IDbConnection connection) {
    //         _connection = connection;
    //     }
    //
    //     public void BeginTran() {
    //         IDbTransaction tran = _connection.BeginTransaction();
    //         _transactions.Push(tran);
    //     }
    //
    //     public void Commit() {
    //         _transactions.xFor(tran => {
    //             tran.Commit();
    //         });
    //         
    //         (0, _transactions.Count()).xForeach(i => {
    //             IDbTransaction tran = null;
    //             if (_transactions.TryPop(out tran)) {
    //                 tran.Commit();
    //             }
    //         });
    //     }
    //
    //     public void Rollback() {
    //         _transactions.xFor(tran => {
    //             tran.Rollback();
    //         });
    //         
    //         (0, _transactions.Count()).xForeach(i => {
    //             IDbTransaction tran = null;
    //             if (_transactions.TryPop(out tran)) {
    //                 tran.Rollback();
    //             }
    //         });
    //     }
    //     
    // }
}
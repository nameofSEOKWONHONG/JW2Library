using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using eXtensionSharp;

namespace JWLibrary.Database {
    /// <summary>
    ///     Database Client Extension
    /// </summary>
    public static partial class JdbClientExtension {
        /// <summary>
        ///     bulk insert, use datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="bulkDatas"></param>
        /// <param name="tableName"></param>
        public static void BulkInsert<T>(this IDbConnection connection, IEnumerable<T> bulkDatas,
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

        public static async Task BulkInsertAsync<T>(this IDbConnection connection, IEnumerable<T> bulkDatas,
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
    public static partial class JdbClientExtension {
        #region [self impletment func method]

        /// <summary>
        ///     you can execute func method, use dbconnection, any code. (use dapper, ef, and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static void DbExecutor(this IDbConnection connection, Action<IDbConnection> action, bool isTran = false) {
            try {
                JTransaction.Instance.Add(connection, isTran);
                connection.xOpen();
                action(connection);
                JTransaction.Instance.Commit(connection);
            }
            catch {
                JTransaction.Instance.Rollback(connection);
                throw;
            }
            finally {
                connection.Close();
            }
        }

        public static void DbExecutor(this Tuple<IDbConnection, IDbConnection> connections,
            Action<IDbConnection, IDbConnection> action, bool isTran = false) {
            try {
                JTransaction.Instance.Adds(new[] {connections.Item1, connections.Item2}, isTran);

                connections.Item1.xOpen();
                connections.Item2.xOpen();

                action(connections.Item1, connections.Item2);

                JTransaction.Instance.Commits(new[] {connections.Item1, connections.Item2});
            }
            catch {
                JTransaction.Instance.Rollbacks(new[]{connections.Item1, connections.Item2});
                throw;
            }
            finally {
                connections.Item1.xClose();
                connections.Item2.xClose();
            }
        }

        /// <summary>
        ///     async execute(select, update, delete, insert) db, use any code on func method. (use dapper, ef and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task DbExecutorAsync(this IDbConnection connection, Func<IDbConnection, Task> func, bool isTran = false) {
            try {
                JTransaction.Instance.Add(connection, isTran);
                connection.xOpen();

                await func(connection);
                
                JTransaction.Instance.Commit(connection);
            }
            catch {
                JTransaction.Instance.Rollback(connection);
                throw;
            }
            finally {
                connection.xClose();
            }
        }

        public static async void DbExecutorAsync(this Tuple<IDbConnection, IDbConnection> connections,
            Func<IDbConnection, IDbConnection, Task> func, bool isTran = false) {
            try {
                JTransaction.Instance.Adds(new[] {connections.Item1, connections.Item2}, isTran);
                connections.Item1.xOpen();
                connections.Item2.xOpen();

                await func(connections.Item1, connections.Item2);
                
                JTransaction.Instance.Commits(new[]{connections.Item1, connections.Item2});
            }
            catch {
                JTransaction.Instance.Rollbacks(new[] {connections.Item1, connections.Item2});
                throw;
            }
            finally {
                connections.Item1.xClose();
                connections.Item2.xClose();
            }
        }

        public static void xOpen(this IDbConnection connection) {
            if(connection.State != ConnectionState.Open) connection.Open();
        }

        public static void xClose(this IDbConnection connection) {
            if(connection.State == ConnectionState.Open) connection.Close();
        }

        #endregion [self impletment func method]
    }

    public class JTransaction {
        private static Lazy<JTransaction> _instance = new Lazy<JTransaction>(() => new JTransaction());

        public static JTransaction Instance {
            get {
                return _instance.Value;
            }
        }
        
        public ConcurrentDictionary<int, IDbTransaction>
            _transactions = new ConcurrentDictionary<int, IDbTransaction>();

        public JTransaction() {
            
        }

        public void Add(IDbConnection connection, bool isTran = false) {
            if (isTran) {
                var result = _transactions.TryAdd(connection.GetHashCode(), connection.BeginTransaction());
                if (result.xIsFalse()) {
                    Trace.WriteLine($"transaction add failed : {connection.GetHashCode()}, {connection.ConnectionString}");
                }
            }
        }

        public void Adds(IEnumerable<IDbConnection> connections, bool isTran = false) {
            if (isTran) {
                connections.xForEach(connection => {
                    Add(connection, isTran);
                });
            }
        }

        public IDbTransaction Get(IDbConnection connection) {
            return _transactions.GetOrAdd(connection.GetHashCode(), (i => _transactions[i] ));
        }

        public void Commit(IDbConnection connection) {
            _transactions.xForEach(tran => {
                if (tran.Key == connection.GetHashCode()) {
                    tran.Value.Commit();
                }
            });
        }

        public void Commits(IEnumerable<IDbConnection> connections) {
            connections.xForEach(connection => {
                Commit(connection);
            });
        }

        public void Rollback(IDbConnection connection) {
            _transactions.xForEach(tran => {
                if (tran.Key == connection.GetHashCode()) {
                    tran.Value.Rollback();
                }
            });
        }

        public void Rollbacks(IEnumerable<IDbConnection> connections) {
            connections.xForEach(connection => {
                Rollback(connection);
            });
        }
        
    }
}
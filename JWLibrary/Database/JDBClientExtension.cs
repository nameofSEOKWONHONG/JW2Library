using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JWLibrary.Core;

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
                if (tableName.isNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.jToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.isNullOrEmpty() ? entity.GetType().Name : tableName;
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
                if (tableName.isNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.jToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null)) {
                    bulkCopy.DestinationTableName = tableName.isNullOrEmpty() ? entity.GetType().Name : tableName;
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
        public static void DbExecutor<T>(this IDbConnection connection, Action<IDbConnection> action) {
            try {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                action(connection);
            }
            finally {
                connection.Close();
            }
        }

        public static void DbExecutor(this IDbConnection connection, Action<IDbConnection> action) {
            try {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                action(connection);
            }
            finally {
                connection.Close();
            }
        }

        public static void DbExecutor<T>(this Tuple<IDbConnection, IDbConnection> connections,
            Action<IDbConnection, IDbConnection> action) {
            try {
                if (connections.Item1.State != ConnectionState.Open)
                    connections.Item1.Open();

                if (connections.Item2.State != ConnectionState.Open)
                    connections.Item2.Open();

                action(connections.Item1, connections.Item2);
            }
            finally {
                connections.Item1.Close();
                connections.Item2.Close();
            }
        }

        /// <summary>
        ///     async execute(select, update, delete, insert) db, use any code on func method. (use dapper, ef and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task DbExecutorAsync<T>(this IDbConnection connection, Func<IDbConnection, Task> func) {
            try {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                await func(connection);
            }
            finally {
                connection.Close();
            }
        }

        public static async void DbExecutorAsync<T>(this Tuple<IDbConnection, IDbConnection> connections,
            Func<IDbConnection, IDbConnection, Task> func) {
            try {
                if (connections.Item1.State != ConnectionState.Open)
                    connections.Item1.Open();

                if (connections.Item2.State != ConnectionState.Open)
                    connections.Item2.Open();

                await func(connections.Item1, connections.Item2);
            }
            finally {
                connections.Item1.Close();
                connections.Item2.Close();
            }
        }

        #endregion [self impletment func method]
    }
}
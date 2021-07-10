using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using eXtensionSharp;
using Microsoft.Data.SqlClient;

namespace JWLibrary.Database
{
    /// <summary>
    ///     Database Client Extension
    ///     TODO:Transaction Commit 부분 변경해야 함.
    /// </summary>
    public class JDbBulkExtension
    {
        /// <summary>
        ///     bulk insert, use datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="bulkDatas"></param>
        /// <param name="tableName"></param>
        public void BulkInsert<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new()
        {
            try
            {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task BulkInsertAsync<T>(IDbConnection connection, IEnumerable<T> bulkDatas,
            string tableName = null)
            where T : class, new()
        {
            try
            {
                if (tableName.xIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.xToDateTable();

                using (var bulkCopy = new SqlBulkCopy((SqlConnection) connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = tableName.xIsNullOrEmpty() ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties())
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);

                    connection.Open();
                    await bulkCopy.WriteToServerAsync(dt);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
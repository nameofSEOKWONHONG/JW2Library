using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.Core {
    public static class JCollection {
        #region [for & foreach]

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void jForeach<T>(this IEnumerable<T> iterator, Action<T> action) {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++) {
                action(srcs[i]);
                if (i % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
            }
        }

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void jForeach<T>(this IEnumerable<T> iterator, Action<T, int> action) {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.jForeach(item => {
                action(item, index);
                index++;
            });
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="func"></param>
        public static void jForeach<T>(this IEnumerable<T> iterator, Func<T, bool> func) {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++) {
                var isBreak = !func(srcs[i]);
                if (isBreak) break;
                
                if (i % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
            }
        }
        
        public static void jForeach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func) {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var srcs = iterator.ToArray();
            for (var i = 0; i < srcs.Length; i++) {
                var isBreak = !func(srcs[i], i);
                if(isBreak) break;
                
                if (i % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
            }
        }
        
        public static async Task jForeachAsync<T>(this IEnumerable<T> iterator, Func<T, Task> func) {
            foreach (var value in iterator)
            {
                await func(value);
            }
        }

        #endregion [for & foreach]

        #region [Datatable & DataReader]

        public static DataTable jToDateTable<T>(this IEnumerable<T> entities)
            where T : class, new() {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            entities.jForeach(item => {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }

        public static T fromReaderToObject<T>(this IDataReader reader)
            where T : class, new() {
            var properties = typeof(T).GetProperties().toList();

            var newItem = new T();

            Enumerable.Range(0, reader.FieldCount - 1).jForeach(i => {
                if (!reader.IsDBNull(i)) {
                    var property = properties.Where(m => m.Name.Equals(reader.GetName(i))).first();
                    if (property.jIsNotNull())
                        if (reader.GetFieldType(i).Equals(property.PropertyType))
                            property.SetValue(newItem, reader[i]);
                }
            });

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }
}
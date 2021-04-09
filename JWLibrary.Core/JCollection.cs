using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace JWLibrary.Core {
    public static class JCollection {
        #region [for & foreach]

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void forEach<T>(this IEnumerable<T> iterator, Action<T> action) {
            if (iterator.count() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                action(item);

                if (index % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
                index++;
            }
        }

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void forEach<T>(this IEnumerable<T> iterator, Action<T, int> action) {
            if (iterator.count() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.forEach(item => {
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
        public static void forEach<T>(this IEnumerable<T> iterator, Func<T, bool> func) {
            if (iterator.count() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                var isBreak = !func(item);
                if (isBreak) break;

                if (index % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
                index++;
            }
        }
        
        
        public static void forEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func) {
            if (iterator.count() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                var isBreak = !func(item, index);
                if (isBreak) break;

                if (index % JConst.LOOP_LIMIT == 0)
                    JConst.setInterval(JConst.SLEEP_INTERVAL);
                index++;
            }
        }

        #endregion [for & foreach]

        #region [Datatable & DataReader]

        public static DataTable jToDataTable<T>(this IEnumerable<T> entities)
            where T : class, new() {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            entities.forEach(item => {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }

        public static T jToObject<T>(this IDataReader reader)
            where T : class, new() {
            var properties = typeof(T).GetProperties().toList();

            var newItem = new T();

            Enumerable.Range(0, reader.FieldCount - 1).forEach(i => {
                if (!reader.IsDBNull(i)) {
                    var property = properties.Where(m => m.Name.Equals(reader.GetName(i))).first();
                    if (property.isNotNull())
                        if (reader.GetFieldType(i).Equals(property.PropertyType))
                            property.SetValue(newItem, reader[i]);
                }
            });

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }
}
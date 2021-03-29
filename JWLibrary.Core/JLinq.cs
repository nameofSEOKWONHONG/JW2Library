using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    public static class JLinq {
        public static int jCount<T>(this IEnumerable<T> enumerable) {
            return enumerable.AsValueEnumerable().Count();
        }

        public static IEnumerable<T> jToList<T>(this IEnumerable<T> enumerable) {
            return enumerable.jIfNotNull(x => x, new JList<T>());
        }

        public static T[] jToArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.jIsNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }

        public static T jFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T jLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }

        public static string jIfNullOrEmpty(this string str, Func<string, string> func) {
            if (str.jIsNullOrEmpty()) return func(str);
            return str;
        }

        public static T jIfNull<T>(this T obj, Func<T, T> predicate) {
            if (predicate.jIsNull()) return predicate(obj);
            return obj;
        }

        public static void jIfNull<T>(this T obj, Action action) {
            if (obj.jIsNull()) action();
        }

        public static void jIfNotNull<T>(this T obj, Action action) {
            if (obj.jIsNotNull()) action();
        }

        public static T jIfNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue)
            where T : class {
            if (obj.jIsNotNull()) return predicate(obj);
            return defaultValue;
        }

        public static IEnumerable<T> jWhere<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class {
            if (enumerable.jIsEmpty()) {
                enumerable = new JList<T>();
                return enumerable;
            }

            return enumerable.AsValueEnumerable().Where(predicate);
        }

        public static IEnumerable<T> jSelect<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class {
            return enumerable.AsValueEnumerable().Select(predicate);
        }
    }
}
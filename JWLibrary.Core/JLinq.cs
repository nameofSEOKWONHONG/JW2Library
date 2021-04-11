using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    public static class JLinq {
        public static int count<T>(this IEnumerable<T> enumerable) {
            return enumerable.AsValueEnumerable().Count();
        }

        public static IEnumerable<T> toList<T>(this IEnumerable<T> enumerable) {
            return enumerable.ifNotNull(x => x, new JList<T>());
        }

        public static T[] toArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.isNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }

        public static T first<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.isNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T last<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.isNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }

        public static string ifNullOrEmpty(this string str, Func<string, string> func) {
            if (str.isNullOrEmpty()) return func(str);
            return str;
        }

        public static string ifNotNullOrEmpty(this string str, Func<string, string> func) {
            if (!str.isNullOrEmpty()) return func(str);
            return str;
        }

        public static T ifNull<T>(this T obj, Func<T, T> predicate) {
            if (predicate.isNull()) return predicate(obj);
            return obj;
        }

        public static T ifNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue) {
            if (obj.isNotNull()) return predicate(obj);
            return defaultValue;
        }

        public static IEnumerable<T> where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class {
            if (enumerable.isEmpty()) {
                enumerable = new JList<T>();
                return enumerable;
            }

            return enumerable.AsValueEnumerable().Where(predicate);
        }

        public static IEnumerable<T> select<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class {
            return enumerable.AsValueEnumerable().Select(predicate);
        }
    }
}
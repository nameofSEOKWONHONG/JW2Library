using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    public static class JObj {
        public static void ifTrue(this bool obj, Action action) {
            if (obj) action();
        }

        public static void ifFalse(this bool obj, Action action) {
            if (!obj) action();
        }

        public static bool isTrue(this bool obj) {
            return obj.Equals(true);
        }

        public static bool isFalse(this bool obj) {
            return obj.Equals(false);
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

        public static bool isNull(this object obj) {
            if (obj == null) return true;
            return false;
        }

        public static bool isNotNull(this object obj) {
            if (obj == null) return false;
            return true;
        }

        public static bool isEmpty(this object obj) {
            if (obj.isNull()) {
                return true;
            }
            else if (obj is string) {
                if ((obj as string).isNullOrEmpty()) return true;    
            }
            else if (obj is ICollection) {
                if ((obj as ICollection).Count > 0)
                    return true;
            }

            return false;
        }

        public static string toValue(this string src, string @default = null) {
            return src.ifNullOrEmpty(x => @default);
        }

        public static string toValue(this object src, object @default = null) {
            return Convert.ToString(src);
        }
        
        public static T toValue<T>(this object src, object @deafult = null) {
            if (src is string) {
                if (string.IsNullOrEmpty(src as string)) {
                    return (T)Convert.ChangeType(@deafult, typeof(T));
                }
            }
            return (T)Convert.ChangeType(src, typeof(T));
        }
        
        public static T toValue<T>(this string src, Nullable<T> @default = null) where T : struct, Enum {
            return src.jStringToEnum<T>(@default.Value);
        }

        public static string toValue(this Enum src, Enum @default = null) {
            return src.isNull() ? (@default == null ? string.Empty : @default.jEnumToString()) : src.jEnumToString();
        }

        public static bool isEquals<T>(this T src, T compare) {
            return src.Equals(compare);
        }
        
        public static bool isEquals<T>(this T src, IEnumerable<T> compares)
            where T : struct {
            var isEqual = false;
            compares.forEach(item => {
                isEqual = item.isEquals(src);
                return !isEqual;
            });

            return isEqual;
        }

        public static bool isEquals<T>(this IEnumerable<T> srcs, T compare) 
            where T: struct {
            return compare.isEquals(srcs);
        }

        

        public static bool isEquals(this string src, string compare) {
            return src.Equals(compare);
        }
        
        public static bool isEquals(this string src, IEnumerable<string> compares) {
            var isEqaul = false;
            compares.forEach(item => {
                isEqaul = src.isEquals(item);
                return !isEqaul;
            });

            return isEqaul;
        }

        public static bool isEquals(this IEnumerable<string> srcs, string compare) {
            return compare.isEquals(srcs);
        }
        
        public static T first<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.isNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T last<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.isNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }        
        
        public static IEnumerable<T> toList<T>(this IEnumerable<T> enumerable) {
            return enumerable.ifNotNull(x => x, new JList<T>());
        }

        public static T[] toArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.isNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }        
    }
}
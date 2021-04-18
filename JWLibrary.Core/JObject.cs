using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    public static class JObj {
        public static void jIfTrue(this bool obj, Action action) {
            if (obj) action();
        }

        public static void jIfFalse(this bool obj, Action action) {
            if (!obj) action();
        }

        public static bool jIsTrue(this bool obj) {
            return obj.Equals(true);
        }

        public static bool jIsFalse(this bool obj) {
            return obj.Equals(false);
        }
        
        public static string jIfNullOrEmpty(this string str, Func<string, string> func) {
            if (str.isNullOrEmpty()) return func(str);
            return str;
        }

        public static string jIfNotNullOrEmpty(this string str, Func<string, string> func) {
            if (!str.isNullOrEmpty()) return func(str);
            return str;
        }

        public static T jIfNull<T>(this T obj, Func<T, T> predicate) {
            if (predicate.jIsNull()) return predicate(obj);
            return obj;
        }

        public static T jIfNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue) {
            if (obj.jIsNotNull()) return predicate(obj);
            return defaultValue;
        }        

        public static bool jIsNull(this object obj) {
            if (obj == null) return true;
            return false;
        }

        public static bool jIsNotNull(this object obj) {
            if (obj == null) return false;
            return true;
        }

        public static bool jIsEmpty(this object obj) {
            if (obj.jIsNull()) {
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

        public static string jToValue(this string src, string @default = null) {
            return src.jIfNullOrEmpty(x => @default);
        }

        public static string jToValue(this object src, object @default = null) {
            return Convert.ToString(src);
        }
        
        public static T jToValue<T>(this object src, object @deafult = null) {
            if (src is string) {
                if (string.IsNullOrEmpty(src as string)) {
                    return (T)Convert.ChangeType(@deafult, typeof(T));
                }
            }
            return (T)Convert.ChangeType(src, typeof(T));
        }
        
        public static T jToValue<T>(this string src, Nullable<T> @default = null) where T : struct, Enum {
            return src.jStringToEnum<T>(@default.Value);
        }

        public static string jToValue(this Enum src, Enum @default = null) {
            return src.jIsNull() ? (@default == null ? string.Empty : @default.jEnumToString()) : src.jEnumToString();
        }

        public static bool jIsEquals<T>(this T src, T compare) {
            return src.Equals(compare);
        }
        
        public static bool jIsEquals<T>(this T src, IEnumerable<T> compares)
            where T : struct {
            var isEqual = false;
            compares.jForeach(item => {
                isEqual = item.jIsEquals(src);
                return !isEqual;
            });

            return isEqual;
        }

        public static bool jIsEquals<T>(this IEnumerable<T> srcs, T compare) 
            where T: struct {
            return compare.jIsEquals(srcs);
        }

        

        public static bool jIsEquals(this string src, string compare) {
            return src.Equals(compare);
        }
        
        public static bool jIsEquals(this string src, IEnumerable<string> compares) {
            var isEqaul = false;
            compares.jForeach(item => {
                isEqaul = src.jIsEquals(item);
                return !isEqaul;
            });

            return isEqaul;
        }

        public static bool jIsEquals(this IEnumerable<string> srcs, string compare) {
            return compare.jIsEquals(srcs);
        }
        
        public static T first<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return enumerable.AsValueEnumerable().FirstOrDefault(predicate);

            return enumerable.AsValueEnumerable().FirstOrDefault();
        }

        public static T last<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return enumerable.AsValueEnumerable().LastOrDefault(predicate);

            return enumerable.AsValueEnumerable().LastOrDefault();
        }        
        
        public static IEnumerable<T> toList<T>(this IEnumerable<T> enumerable) {
            return enumerable.jIfNotNull(x => x, new JList<T>());
        }

        public static T[] toArray<T>(this IEnumerable<T> enumerable) where T : new() {
            if (enumerable.jIsNull()) return new T[0];
            return enumerable.AsValueEnumerable().ToArray();
        }        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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
    }
}
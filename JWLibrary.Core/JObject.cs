using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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
                if ((obj as string).jIsNullOrEmpty()) return true;    
            }
            else if (obj is ICollection) {
                if ((obj as ICollection).Count > 0)
                    return true;
            }

            return false;
        }

        public static string jValue(this string src, string @default = null) {
            return src.jIfNullOrEmpty(x => @default);
        }

        public static string jValue(this object src, object @default = null) {
            return Convert.ToString(src);
        }
        
        public static T jValue<T>(this object src, object @deafult = null) {
            if (src is string) {
                if (string.IsNullOrEmpty(src as string)) {
                    return (T)Convert.ChangeType(@deafult, typeof(T));
                }
            }
            return (T)Convert.ChangeType(src, typeof(T));
        }
        
        public static T jValue<T>(this string src, Nullable<T> @default = null) where T : struct, Enum {
            return src.jStringToEnum<T>(@default.Value);
        }

        public static string jValue(this Enum src, Enum @default = null) {
            return src.jIsNull() ? (@default == null ? string.Empty : @default.jEnumToString()) : src.jEnumToString();
        }

        public static int Define(string src) {
            return 0;
        }
    }
}
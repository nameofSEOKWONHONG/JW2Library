using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace JWLibrary.Core {
    public static class JLinq {
        public static int @count<T>(this IEnumerable<T> enumerable) {
            return enumerable.AsValueEnumerable().Count();
        }
        
        public static IEnumerable<T> @where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
            where T : class {
            if (enumerable.isEmpty()) {
                enumerable = new JList<T>();
                return enumerable;
            }

            return enumerable.AsValueEnumerable().Where(predicate);
        }

        public static IEnumerable<T> @select<T>(this IEnumerable<T> enumerable, Func<T, T> predicate)
            where T : class {
            return enumerable.AsValueEnumerable().Select(predicate);
        }
    }
}
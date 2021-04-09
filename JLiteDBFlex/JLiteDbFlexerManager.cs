using System;
using System.Linq;
using JWLibrary.Core;
using Nito.AsyncEx;

namespace JLiteDBFlex {
    public class JLiteDbFlexerManager {
        private static readonly AsyncLock _mutex = new();

        private static readonly JHDictionary<Type, IJLiteDbFlexer> _instanceMap =
            new();

        public static JLiteDbFlexer<T> Create<T>() where T : class {
            using (_mutex.Lock()) {
                var exists = _instanceMap.FirstOrDefault(m => m.Key == typeof(T));
                if (exists.isNotNull()) return exists.Value as JLiteDbFlexer<T>;

                var newInstance = new JLiteDbFlexer<T>();
                if (newInstance.isNotNull()) _instanceMap.Add(typeof(T), newInstance);

                return newInstance;
            }
        }

        public static void Distroy() {
            _instanceMap.forEach(instance => {
                if (instance.Value.isNotNull()) instance.Value.LiteDatabase?.Dispose();
            });

            _instanceMap.Clear();
        }
    }
}
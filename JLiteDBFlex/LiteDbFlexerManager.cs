using System;
using System.Linq;
using eXtensionSharp;
using LiteDB;
using Nito.AsyncEx;

namespace JLiteDBFlex {
    /// <summary>
    /// litedb flexer manager
    /// create instance of litedb
    /// </summary>
    public sealed class LiteDbFlexerManager {
        private readonly AsyncLock _mutex = new();

        private readonly XHDictionary<Type, ILiteDbFlexer> _instanceMap =
            new();

        private static Lazy<LiteDbFlexerManager> _instance =
            new Lazy<LiteDbFlexerManager>(() => new LiteDbFlexerManager());

        public static LiteDbFlexerManager Instance {
            get {
                return _instance.Value;
            }
        }

        /// <summary>
        /// create instance of litedb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ValueTuple</returns>
        public (ILiteCollection<T> LiteCollection, string TableName, string FileName, ILiteDatabase LiteDatabase) Create<T>() where T : class {
            var exists = _instanceMap.FirstOrDefault(m => m.Key == typeof(T));
            if (exists.Key.xIsNotNull()) {
                var result = exists.Value as LiteDbFlexer<T>;
                return new(result.LiteCollection, result.TableName, result.FileName, result.LiteDatabase);
            }

            using (_mutex.Lock()) {
                var newInstance = new LiteDbFlexer<T>();
                if (_instanceMap.Keys.Contains(typeof(T)).xIsFalse()) {
                    _instanceMap.Add(typeof(T), newInstance);
                    return new(newInstance.LiteCollection, newInstance.TableName, newInstance.FileName, newInstance
                        .LiteDatabase);                        
                }
            }

            return new (null, null, null, null);
        }

        /// <summary>
        /// litedb dispose.
        /// collection clear.
        /// </summary>
        public void Distroy() {
            _instanceMap.xForEach(instance => {
                if (instance.Value.xIsNotNull()) instance.Value.LiteDatabase?.Dispose();
            });

            _instanceMap.Clear();
        }
    }
}
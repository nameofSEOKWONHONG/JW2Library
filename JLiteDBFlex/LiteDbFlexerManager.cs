using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<string, ILiteDbFlexer> _instanceMap =
            new();

        private static Lazy<LiteDbFlexerManager> _instance =
            new Lazy<LiteDbFlexerManager>(() => new LiteDbFlexerManager());

        public static LiteDbFlexerManager Instance {
            get {
                return _instance.Value;
            }
        }

        /// <summary>
        /// create instance by entity attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>ValueTuple</returns>
        public ILiteDbFlexer Create<T>(ConnectionType type = ConnectionType.Shared) where T : class {
            var fileName = typeof(T).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.FileName);
            if (fileName.xIsNullOrEmpty()) throw new Exception("entity file name attribute is empty.");
            var tableName = typeof(T).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.TableName);
            if (tableName.xIsNullOrEmpty()) throw new Exception("entity table name attribute is empty.");
            
            var exists = _instanceMap.FirstOrDefault(m => m.Key == fileName);
            if (exists.Key.xIsNotNull()) {
                return exists.Value as LiteDbFlexer<T>;
            }
            
            using (_mutex.Lock()) {
                exists = _instanceMap.FirstOrDefault(m => m.Key == fileName);
                if (exists.Key.xIsNotNull()) {
                    return exists.Value as LiteDbFlexer<T>;
                }
                
                var newInstance = new LiteDbFlexer<T>(type);
                if (_instanceMap.Keys.Contains(fileName).xIsFalse()) {
                    if (_instanceMap.TryAdd(fileName, newInstance)) {
                        return newInstance;
                    } 
                }
            }

            return null;
        }

        /// <summary>
        /// create instance by manual
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ILiteDbFlexer Create<T>(string fileName, string tableName, ConnectionType type = ConnectionType.Shared) where T : class {
            if (fileName.xIsNullOrEmpty()) throw new Exception("file name is empty");
            if (tableName.xIsNullOrEmpty()) throw new Exception("table name is empty");
            var exists = _instanceMap.FirstOrDefault(m => m.Key == fileName);
            if (exists.Key.xIsNotNull()) {
                return exists.Value as LiteDbFlexer;
            }

            using (_mutex.Lock()) {
                exists = _instanceMap.FirstOrDefault(m => m.Key == fileName);
                if (exists.Key.xIsNotNull()) {
                    return exists.Value as LiteDbFlexer;
                }
                
                var newInstance = new LiteDbFlexer<T>(type);
                if (_instanceMap.Keys.Contains(fileName).xIsFalse()) {
                    if (_instanceMap.TryAdd(fileName, newInstance)) {
                        return newInstance;
                    } 
                }
            }
            
            return null;
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
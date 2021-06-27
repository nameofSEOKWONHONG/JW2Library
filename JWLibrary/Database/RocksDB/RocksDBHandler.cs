using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using eXtensionSharp;
using IronPython.Modules;
using Nito.AsyncEx;

namespace JWLibrary.Database {
    public class RocksDBHandler : IDisposable {
        private static readonly Lazy<RocksDBHandler> _instance =
            new(() => new RocksDBHandler());

        private readonly ConcurrentDictionary<string, RocksDBImpl> _concurrentDbHandlerMaps =
            new();

        public static RocksDBHandler Instance => _instance.Value;

        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly AsyncLock _innerMutex = new AsyncLock();

        #region [dispose]

        public void Dispose() {
            Free();
        }


        #endregion

        #region [public method]

        public RocksDBResult ExecuteCommand(RocksDBRequest request) {
            if (ROCKSDB_COMMAND.Get == request.Command) {
                var getResult = Get(request.Path, request.Key);
                return new RocksDBResult {
                    Key = request.Key,
                    Value = getResult,
                    State = getResult.xIsNullOrEmpty() ? false : true
                };
            }
            else if (ROCKSDB_COMMAND.Put == request.Command) {
                Put(request.Path, request.Key, request.Value);
                return new RocksDBResult {
                    Key = request.Key,
                    Value = request.Value.xSafe(),
                    State = true
                };
            }
            else if (ROCKSDB_COMMAND.Gets == request.Command) {
                var getsResult = Gets(request.Path, request.Keys).xSafe();
                return new RocksDBResult() {
                    KeyValues = getsResult,
                    State = getsResult.xIsNull() ? false : true
                };
            }
            else if (ROCKSDB_COMMAND.Puts == request.Command) {
                Puts(request.Path, request.KeyValues);
                return new RocksDBResult() {
                    State = true
                };
            }
            else if (ROCKSDB_COMMAND.Remove == request.Command) {
                Remove(request.Path, request.Key);
                return new RocksDBResult() {
                    State = true
                };
            }
            else if (ROCKSDB_COMMAND.Removes == request.Command) {
                Removes(request.Path, request.Keys);
                return new RocksDBResult() {
                    State = true
                };
            }

            return new RocksDBResult() {
                State = false,
                StateMsg = $"not implement {request.Command}"
            };
        }

        public bool RemovePath(string path) {
            var exists = _concurrentDbHandlerMaps[path];
            if (exists.xIsNotEmpty()) {
                exists.Dispose();
                try {
                    exists.FullPath.xFileDeleteAll();
                    return true;
                }
                catch (Exception e) {
                    return false;
                }
            }

            return false;
        }
        
        public void Free() {
            _concurrentDbHandlerMaps.xForEach(item => { item.Value.Dispose(); });
        }
        #endregion

        #region [private method]

        private RocksDBImpl GetRocksDBImpl(string path) {
            using (_mutex.Lock()) {
                RocksDBImpl impl = null;
            
                if (_concurrentDbHandlerMaps.TryGetValue(path, out impl)) {
                    return impl;
                }
                else {
                    using (_innerMutex.Lock()) {
                        impl = new RocksDBImpl(path);
                        _concurrentDbHandlerMaps.TryAdd(path, impl);                        
                    }
                }
                return impl;                 
            }
        }

        private void Put(string path, string key, string value) {
            var impl = GetRocksDBImpl(path);
            impl.Put(key, value);
        }

        private string Get(string path, string key) {
            var impl = GetRocksDBImpl(path);
            return impl.Get(key);
        }

        private void Puts(string path, Dictionary<string, string> maps) {
            var impl = GetRocksDBImpl(path);
            impl.Puts(maps);
        }

        private Dictionary<string, string> Gets(string path, IEnumerable<string> keys) {
            var impl = GetRocksDBImpl(path);
            return impl.Gets(keys);
        }

        private void Remove(string path, string key) {
            var impl = GetRocksDBImpl(path);
            impl.Remove(key);
        }

        private void Removes(string path, IEnumerable<string> keys) {
            var impl = GetRocksDBImpl(path);
            impl.Removes(keys);
        }

        #endregion
    }
}
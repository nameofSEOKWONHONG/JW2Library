using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Community.CsharpSqlite;
using eXtensionSharp;
using RocksDbSharp;

namespace JWLibrary.Database {
    public class RocksDbHandler : IDisposable {
        private static Lazy<RocksDbHandler> _instance =
            new Lazy<RocksDbHandler>(() => new RocksDbHandler());
        
        public static RocksDbHandler Instance {
            get {
                return _instance.Value;
            }
        }
        
        private ConcurrentDictionary<string, RocksDbImpl> _concurrentDbHandlerMaps =
            new ConcurrentDictionary<string, RocksDbImpl>();

        public RocksDbHandler() {
            
        }

        public bool CreatePath(string path) {
            var exists = _concurrentDbHandlerMaps[path];
            if (exists.xIsEmpty()) {
                var handler = new RocksDbImpl(path);
                return _concurrentDbHandlerMaps.TryAdd(path, handler);
            }

            return false;
        }

        public void Put(string path, string key, string value) {
            RocksDbImpl handler = null;
            if (_concurrentDbHandlerMaps.TryGetValue(path, out handler)) {
                handler.Put(key, value);
            }
            else {
                handler = new RocksDbImpl(path);
                _concurrentDbHandlerMaps.TryAdd(path, handler);
                handler.Put(key, value);
            }
        }

        public string Get(string path, string key) {
            var exists = _concurrentDbHandlerMaps[path];
            if (exists.xIsNotEmpty()) {
                return exists.Get(key);
            }

            throw new Exception("not find path");
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
            _concurrentDbHandlerMaps.xForEach(item => {
                item.Value.Dispose();
            });
        }

        public void Dispose() {
            Free();
        }
    }
    
    internal class RocksDbImpl : IDisposable {
        private const string PREFIX = "rocksdb";
        private string _dbPath;

        public string FullPath {
            get {
                return _dbPath;
            }
        }

        private RocksDb _rocksDb;
        
        public RocksDbImpl(string dbPath) {
            PREFIX.xToPath().xDirCreateAll();
            _dbPath = $"{PREFIX}\\{dbPath}".xToPath();
            CreateRocksDb();
        }

        public RocksDbImpl CreateRocksDb(bool isCreateIfMissing = true) {
            var options = new DbOptions()
                .SetCreateIfMissing(isCreateIfMissing);
            _rocksDb = RocksDb.Open(options, _dbPath);
            return this;
        }

        public string ExecuteCommand(string cmd, string key, string value) {
            var result = string.Empty;
            switch (cmd.ToUpper()) {
                case "PUT":
                    Put(key, value);
                    break;
                case "GET":
                    result = Get(key);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            return result;
        }

        public void Put(string key, string value) {
            var exists = _rocksDb.Get(key);
            if (exists.xIsEmpty()) {
                _rocksDb.Put(key, value);
            }
        }

        public void PutBytes(string key, string value) {
            var eKey = Encoding.UTF8.GetBytes(key);
            var eVal = Encoding.UTF8.GetBytes(value);
            _rocksDb.Put(eKey, eVal);
        }

        public string Get(string key) {
            return _rocksDb.Get(key);
        }

        public string GetBytes(string key) {
            var eKey = Encoding.UTF8.GetBytes(key);
            var eVal = _rocksDb.Get(eKey);
            return Encoding.UTF8.GetString(eVal);
        }

        public void Remove(string key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null) {
            _rocksDb.Remove(key, cf, writeOptions);
        }

        public void RemoveBytes(string key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null) {
            var eKey = Encoding.UTF8.GetBytes(key);
            _rocksDb.Remove(eKey);
        }
        
        public void Dispose() {
            if (_rocksDb.xIsNotNull()) {
                _rocksDb.Dispose();
            }
        }
    }
}
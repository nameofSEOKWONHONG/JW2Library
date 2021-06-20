using System;
using System.Collections.Generic;
using System.Text;
using eXtensionSharp;
using RocksDbSharp;

namespace RocksGrpcTest {
    public class RocksDbHandler : IDisposable {
        private string _dbPath;
        private RocksDb _rocksDb;

        private Dictionary<string, Func<RocksDb, string, string, string>> _commandMaps = new Dictionary<string, Func<RocksDb, string, string, string>>()
        {
            {
                "PUT", (r, k, v) =>
                {
                    var exists = r.Get(k);
                    if(exists.xIsEmpty())
                    {
                        r.Put(k, v);
                    }

                    return string.Empty;
                }
            },
            {
                "GET", (r, k, v) =>
                {
                    return r.Get(k);
                }
            }
        };
        
        public RocksDbHandler(string dbPath) {
            _dbPath = dbPath;
        }

        public RocksDbHandler CreateRocksDb(bool isCreateIfMissing = true) {
            var options = new DbOptions()
                .SetCreateIfMissing(isCreateIfMissing);
            _rocksDb = RocksDb.Open(options, _dbPath);
            return this;
        }

        public string ExecuteCommand(string cmd, string key, string value)
        {
            return _commandMaps[cmd.ToUpper()].Invoke(_rocksDb, key, value);
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
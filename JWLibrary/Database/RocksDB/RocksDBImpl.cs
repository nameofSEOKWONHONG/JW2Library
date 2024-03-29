﻿using System;
using System.Collections.Generic;
using System.Text;
using eXtensionSharp;
using RocksDbSharp;

namespace JWLibrary.Database
{
    internal class RocksDBImpl : IDisposable
    {
        /// <summary>
        ///     prefix path (base path)
        /// </summary>
        private const string PREFIX = "rocksdb";

        /// <summary>
        ///     save path (local path)
        /// </summary>
        private readonly string _dbPath;

        /// <summary>
        ///     rocksdb instance
        /// </summary>
        private RocksDb _db;

        #region [ctor]

        public RocksDBImpl(string dbPath)
        {
            PREFIX.xToPath().xDirCreateAll();
            _dbPath = $"{PREFIX}\\{dbPath}".xToPath();
            CreateRocksDB();
        }

        #endregion

        /// <summary>
        ///     full path (add filename)
        /// </summary>
        public string FullPath => _dbPath;

        #region [dispose]

        public void Dispose()
        {
            if (_db.xIsNotNull()) _db.Dispose();
        }

        #endregion

        #region [private method]

        private void CreateRocksDB(bool isCreateIfMissing = true)
        {
            var options = new DbOptions()
                .SetCreateIfMissing(isCreateIfMissing);
            _db = RocksDb.Open(options, _dbPath);
        }

        #endregion

        #region [public method]

        public void Put(string key, string value)
        {
            var exists = _db.Get(key);
            if (exists.xIsEmpty()) PutBytes(key, value);
        }

        private void PutBytes(string key, string value)
        {
            var eKey = Encoding.UTF8.GetBytes(key);
            var eVal = Encoding.UTF8.GetBytes(value);
            _db.Put(eKey, eVal);
        }

        public void Puts(Dictionary<string, string> maps)
        {
            var batch = new WriteBatch();
            maps.xForEach(keyvalues => { batch.Put(keyvalues.Key, keyvalues.Value); });
            _db.Write(batch);
            batch.Dispose();
        }

        public void PutsBytes(Dictionary<string, string> maps)
        {
            var batch = new WriteBatch();
            maps.xForEach(keyvalues =>
            {
                var bytesKey = keyvalues.Key.xToBytes();
                var bytesValue = keyvalues.Value.xToBytes();
                batch.Put(bytesKey, bytesValue);
            });
            _db.Write(batch);
            batch.Dispose();
        }

        public string Get(string key)
        {
            return GetBytes(key);
        }

        private string GetBytes(string key)
        {
            var eKey = Encoding.UTF8.GetBytes(key);
            var eVal = _db.Get(eKey);
            if (eVal.xIsNull()) return string.Empty;
            return Encoding.UTF8.GetString(eVal);
        }

        public Dictionary<string, string> Gets(IEnumerable<string> keys)
        {
            var maps = new Dictionary<string, string>();

            if (keys.xIsEmpty()) return maps;

            var bytesKeys = new List<byte[]>();
            keys.xForEach(key => { bytesKeys.Add(key.xToBytes()); });

            var keyvalues = _db.MultiGet(bytesKeys.ToArray());
            keyvalues.xForEach(kv => { maps.Add(kv.Key.xToString(), kv.Value.xToString()); });

            return maps;
        }

        public void Remove(string key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            RemoveBytes(key, cf, writeOptions);
        }

        public void RemoveBytes(string key, ColumnFamilyHandle cf = null, WriteOptions writeOptions = null)
        {
            var eKey = Encoding.UTF8.GetBytes(key);
            _db.Remove(eKey);
        }

        public void Removes(IEnumerable<string> keys)
        {
            keys.xForEach(key => { Remove(key); });
        }

        #endregion
    }
}
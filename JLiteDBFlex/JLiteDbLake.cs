using System;
using System.Collections.Generic;
using JWLibrary.Core;
using LiteDB;

namespace JLiteDBFlex {
    public interface IJLiteDbLakeOption {
        string FileName { get; set; }
        JHDictionary<string, bool> Indexes { get; set; }
        Type DatabaseType { get; }
    }

    public class JLiteDbLakeOption<T> : IJLiteDbLakeOption {
        public string FileName { get; set; }
        public JHDictionary<string, bool> Indexes { get; set; }

        public Type DatabaseType => typeof(T);
    }

    public class JLiteDbLake : IDisposable {
        private static readonly JHDictionary<IJLiteDbLakeOption, LiteDatabase> _litedbKeyValuePairs =
            new();

        public JLiteDbLake() {
            
        }

        ~JLiteDbLake() {
            Dispose();
        }

        public LiteDatabase GetOrAdd<T>(JLiteDbLakeOption<T> option)
            where T : class {
            if (option.isNull()) throw new Exception("option is null");
            if (option.FileName.isEmpty()) throw new Exception("filename is empty");

            var exists = _litedbKeyValuePairs.first(m => m.Key.FileName == option.FileName);
            if (exists.Key.isEmpty() == false) return exists.Value;

            var litedb = new LiteDatabase(option.FileName);
            if (litedb.isNotNull()) {
                if (option.Indexes.isNotNull())
                    option.Indexes.forEach(item => { litedb.GetCollection<T>().EnsureIndex(item.Key, item.Value); });

                _litedbKeyValuePairs.Add(option, litedb);
            }

            return litedb;
        }

        public IEnumerable<IJLiteDbLakeOption> GetOptions() {
            return _litedbKeyValuePairs.Keys;
        }

        public void Remove(IJLiteDbLakeOption option) {
            var exists = _litedbKeyValuePairs.first(m => m.Key.FileName == option.FileName);
            if (exists.Key.FileName.isEmpty() == false) {
                exists.Value.Dispose();
                _litedbKeyValuePairs.Remove(exists.Key);
            }
        }
        
        public void Dispose() {
            _litedbKeyValuePairs.forEach(item => { item.Value.Dispose(); });

            _litedbKeyValuePairs.Clear();
        }
    }
}
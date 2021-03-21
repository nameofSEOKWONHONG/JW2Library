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

        public void Dispose() {
            _litedbKeyValuePairs.jForEach(item => { item.Value.Dispose(); });

            _litedbKeyValuePairs.Clear();
        }

        ~JLiteDbLake() {
            Dispose();
        }

        public static LiteDatabase GetOrAdd<T>(JLiteDbLakeOption<T> option)
            where T : class {
            if (option.jIsNull()) throw new Exception("option is null");
            if (option.FileName.jIsEmpty()) throw new Exception("filename is empty");

            var exists = _litedbKeyValuePairs.jFirst(m => m.Key.FileName == option.FileName);
            if (exists.Key.jIsEmpty() == false) return exists.Value;

            var litedb = new LiteDatabase(option.FileName);
            if (litedb.jIsNotNull()) {
                if (option.Indexes.jIsNotNull())
                    option.Indexes.jForEach(item => { litedb.GetCollection<T>().EnsureIndex(item.Key, item.Value); });

                _litedbKeyValuePairs.Add(option, litedb);
            }

            return litedb;
        }

        public static IEnumerable<IJLiteDbLakeOption> GetOptions() {
            return _litedbKeyValuePairs.Keys;
        }

        public static void Remove(IJLiteDbLakeOption option) {
            var exists = _litedbKeyValuePairs.jFirst(m => m.Key.FileName == option.FileName);
            if (exists.Key.FileName.jIsEmpty() == false) {
                exists.Value.Dispose();
                _litedbKeyValuePairs.Remove(exists.Key);
            }
        }
    }
}
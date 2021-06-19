using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using NUnit.Framework;
using RocksDbSharp;

namespace RocksGrpcTest {
    public class Tests {
        private RocksDb _rocksDb;
        [SetUp]
        public void Setup() {
            var path = "rocksdb".xToPath();
            var options = new DbOptions()
                .SetCreateIfMissing(true);

            _rocksDb = RocksDb.Open(options, path);
        }

        [Test]
        public void create_rocksdb_test() {
            Parallel.ForEach(Enumerable.Range(1, 10), i => {
                // Using strings below, but can also use byte arrays for both keys and values
                // much care has been taken to minimize buffer copying
                _rocksDb.Put($"key{i}", "value");
                string value = _rocksDb.Get($"key{i}");
                Assert.AreEqual("value", value);
                _rocksDb.Remove($"key{i}");
            });

            
            _rocksDb.Dispose();
        }
    }
}
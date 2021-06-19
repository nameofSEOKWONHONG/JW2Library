using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using Nito.AsyncEx;
using NUnit.Framework;

namespace RocksGrpcTest {
    public class RocksDBHandlerTest : IDisposable {
        private RocksDbHandler _rocksDbHandler;
        private readonly AsyncLock _mutex = new AsyncLock();
        
        [SetUp]
        public void Setup() {
            var path = "rocksdb".xToPath();
            _rocksDbHandler = new RocksDbHandler(path).CreateRocksDb();
        }

        [Test]
        public void parallel_put_test_without_lock() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, 100), i => {
                using (_mutex.Lock()) {
                    _rocksDbHandler.Put("test", "value");
                    _rocksDbHandler.Get("test");
                }
            });
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
        }

        [Test]
        public void parallel_put_test() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, 100), i => {
                _rocksDbHandler.Put("test", "value");
                _rocksDbHandler.Get("test");
            });
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
        }

        [Test]
        public void get_test() {
            var v = _rocksDbHandler.Get("test");
            Assert.AreEqual("value", v);
        }

        public void Dispose() {
            Console.WriteLine("call dispose");
            if (_rocksDbHandler.xIsNotNull()) {
                _rocksDbHandler.Dispose();
            }
        }
    }
}
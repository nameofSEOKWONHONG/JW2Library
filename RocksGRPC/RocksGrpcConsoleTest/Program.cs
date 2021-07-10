using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using RocksGrpcNetClient;

namespace RocksGrpcConsoleTest {
    class Program {
        static void Main(string[] args) {
            Parallel.ForEach(Enumerable.Range(1, 100), i => {
            //Enumerable.Range(1, 100).ToList().ForEach(item => {
                Stopwatch sw = Stopwatch.StartNew();
                var result = JCacheHandler.Instance.GetOrAdd<string, string>("test", "asdfasdf", ENUM_CACHE_TYPE.ROCKSDB);
                Console.WriteLine(result.xToJson());
            
                JCacheHandler.Instance.ResetCache<string>("test", ENUM_CACHE_TYPE.ROCKSDB);

                sw.Stop();
                Console.WriteLine($"execution time {sw.Elapsed.TotalSeconds}");
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
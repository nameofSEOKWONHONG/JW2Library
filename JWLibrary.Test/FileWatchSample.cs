using System;
using JWLibrary.Utils.Files;

namespace JCoreSvcTest {
    internal class FileWatchSample {
        private static void Test() {
            var fswProvider = new FileSystemWatcherProvider(@"D:\database");
            fswProvider.Created((s, e, fi) => {
                Console.WriteLine(e.ChangeType.ToString());
                Console.WriteLine(e.FullPath);
            }).Changed((s, e, fi) => {
                Console.WriteLine(e.ChangeType.ToString());
                Console.WriteLine(e.FullPath);
            }).Start();

            Console.ReadLine();

            fswProvider.Stop();
            fswProvider.Dispose();
        }
    }
}
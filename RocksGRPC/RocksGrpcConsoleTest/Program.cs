using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using Grpc.Net.Client;
using RocksGrpcNet;

namespace RocksGrpcConsoleTest {
    class Program {
        static void Main(string[] args) {

            var path = "testdb4";
            var key = "test";
            var value = "value";
            
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client =  new RocksGrpcExecutor.RocksGrpcExecutorClient(channel);
            
            //Parallel.ForEach(Enumerable.Range(1, 10), item => {
            Enumerable.Range(1, 100).ToList().ForEach(item => {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                
                Enumerable.Range(1, 1).ToList().ForEach(i => {
                    //Parallel.ForEach(Enumerable.Range(1, 100), i => {
                    var kkey = $"{key}{i}";
                    var vvalue = $"{value}{i}";
            
                    var reply = client.ExecuteCommandAsync(new RocksGrpcRequest() {
                        Path = path,
                        Command = "PUT",
                        Key = kkey,
                        Value = vvalue
                    }).GetAwaiter().GetResult();
                });
            
                Enumerable.Range(1, 1).ToList().ForEach(i => {
                    //Parallel.ForEach(Enumerable.Range(1, 100), i => {
                    var kkey = $"{key}{i}";
                    var vvalue = $"{value}{i}";
                
                    var result = client.ExecuteCommandAsync(new RocksGrpcRequest() {
                        Path = path,
                        Command = "GET",
                        Key = kkey
                    }).GetAwaiter().GetResult();
                });
                sw.Stop();
                Console.WriteLine("run sec : " + sw.Elapsed.TotalSeconds);
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
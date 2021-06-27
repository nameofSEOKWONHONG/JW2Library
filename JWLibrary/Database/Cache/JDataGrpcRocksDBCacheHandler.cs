using System;
using eXtensionSharp;
using Grpc.Net.Client;
using Newtonsoft.Json;
using RocksGrpcNet;

namespace JWLibrary.Database {
    internal class JDataGrpcRocksDBCacheHandler {
        private static Lazy<JDataGrpcRocksDBCacheHandler> _instance = new Lazy<JDataGrpcRocksDBCacheHandler>(() => new JDataGrpcRocksDBCacheHandler());

        public static JDataGrpcRocksDBCacheHandler Instance {
            get { return _instance.Value; }
        }

        private GrpcChannel _channel = null;
        private RocksGrpcExecutor.RocksGrpcExecutorClient _client = null;

        private JDataGrpcRocksDBCacheHandler() {
            _channel = GrpcChannel.ForAddress("https://localhost:5001");
            _client =  new RocksGrpcExecutor.RocksGrpcExecutorClient(_channel);
        }

        public TResult GetOrAdd<TKey, TResult>(TKey key, TResult result) {
            var getRequest = new RocksGrpcRequest() {
                Key = key.xObjectToJson(),
                Value = result.xObjectToJson(),
                Command = "GET",
                Path = "testdb5"
            };
            var clientResult = _client.ExecuteCommandAsync(getRequest).GetAwaiter().GetResult();

            if (clientResult.State == false) {
                var putRequest = new RocksGrpcRequest() {
                    Key = key.xObjectToJson(),
                    Value = result.xObjectToJson(),
                    Command = "PUT",
                    Path = "testdb5"
                };
                _client.ExecuteCommandAsync(putRequest).GetAwaiter().GetResult();
            }

            return JsonConvert.DeserializeObject<TResult>(clientResult.Value);
        }

        public void ResetCache<TKey>(TKey key) {
            _client.ExecuteCommandAsync(new RocksGrpcRequest() {
                Key = key.xObjectToJson(),
                Command = "REMOVE",
                Path = "testdb5"
            }).GetAwaiter().GetResult();
        }
    }
}
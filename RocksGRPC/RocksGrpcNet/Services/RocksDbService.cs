using System.Collections.Generic;
using System.Threading.Tasks;
using eXtensionSharp;
using Grpc.Core;
using JWLibrary.Database;
using Microsoft.Extensions.Logging;

namespace RocksGrpcNet {
    public class RocksDbService : RocksGrpcExecutor.RocksGrpcExecutorBase {
        private readonly ILogger<RocksDbService> _logger;

        public RocksDbService(ILogger<RocksDbService> logger) {
            _logger = logger;
        }

        public override Task<RocksGrpcReply> ExecuteCommand(RocksGrpcRequest request, ServerCallContext context) {
            var rocksDbRequest = new RocksDBRequest() {
                Path = request.Path,
                Command = ROCKSDB_COMMAND.Parse(request.Command),
                Key = request.Key,
                Value = request.Value,
                Keys = request.Keys,
                KeyValues = new Dictionary<string, string>(request.KeyValues)
            };
            var result = RocksDBHandler.Instance.ExecuteCommand(rocksDbRequest);
            var rocksGrpcReply = new RocksGrpcReply() {
                Key = result.Key.xSafe(),
                Value = result.Value.xSafe(),
                State = result.State.xSafe<bool>(false),
                StateMsg = result.StateMsg.xSafe(),
                KeyValues = { result.KeyValues.xSafe<Dictionary<string, string>>() }
            };
                
            return Task.FromResult(rocksGrpcReply);
        }
    }
}
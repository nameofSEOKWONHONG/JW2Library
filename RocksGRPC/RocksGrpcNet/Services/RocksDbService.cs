using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using JWLibrary.Database;
using Ubiety.Dns.Core.Records;

namespace RocksGrpcNet
{
    public class RocksDbService : RocksGrpcExecutor.RocksGrpcExecutorBase {
        private readonly ILogger<RocksDbService> _logger;

        public RocksDbService(ILogger<RocksDbService> logger)
        {
            _logger = logger;
        }

        public override Task<RocksGrpcReply> ExecuteCommand(RocksGrpcRequest request, ServerCallContext context) {
            var result = string.Empty;
            switch (request.Command.ToUpper()) {
                case "PUT":
                    RocksDbHandler.Instance.Put(request.Path, request.Key, request.Value);
                    result = request.Value;
                    break;
                case "GET":
                    result = RocksDbHandler.Instance.Get(request.Path, request.Key);
                    break;
                case "DELETE":
                    result = RocksDbHandler.Instance.RemovePath(request.Path).ToString();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Task.FromResult(new RocksGrpcReply()
            {
                Result = result
            });
        }
    }
}

using System;
using System.Linq;
using eXtensionSharp;
using JWLibrary.ServiceExecutor;

namespace JWUpdator {
    internal class Program {
        /// <summary>
        ///     updator entry point
        /// </summary>
        /// <param name="version">version no</param>
        /// <param name="debug">0:debug, 1:Release</param>
        private static void Main(int version = 0, int debug = 0) {
            UpdatorDto updatorDto = null;
            using var svc = new ServiceExecutorManager<IUpdatorExistsService>(new UpdatorExistsService());
            svc.SetRequest(o => o.Request = version)
                .OnExecuted(o => {
                    updatorDto = o.Result;
                    return true;
                });

            if (updatorDto.xIsNotNull()) {
                using var updatorSvc = new ServiceExecutorManager<IUpdatorService>(new UpdatorService());
                updatorSvc.AddFilter(o => o.Result.xIsNotNull())
                    .SetRequest(o => o.Request = updatorDto.FileList)
                    .OnExecuted(o => {
                        if (o.Result) Console.WriteLine("downloaded");
                        return true;
                    });
            }
        }
    }
}
using System;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace JWUpdator {
    internal class Program {
        /// <summary>
        ///     updator entry point
        /// </summary>
        /// <param name="version">version no</param>
        /// <param name="debug">0:debug, 1:release</param>
        private static void Main(int version = 0, int debug = 0) {
            UpdatorDto updatorDto = null;
            using var svc = new ServiceExecutorManager<IUpdatorExistsService>(new UpdatorExistsService());
            svc.SetRequest(o => o.Request = version)
                .OnExecuted(o => { updatorDto = o.Result; });

            if (updatorDto.jIsNotNull()) {
                using var updatorSvc = new ServiceExecutorManager<IUpdatorService>(new UpdatorService());
                updatorSvc.AddFilter(o => o.Result.jIsNotNull())
                    .SetRequest(o => o.Request = updatorDto.FileList)
                    .OnExecuted(o => {
                        if (o.Result) Console.WriteLine("downloaded");
                    });
            }
        }
    }
}
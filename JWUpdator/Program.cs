using System;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace JWUpdator {
    class Program {
        /// <summary>
        /// 진입점
        /// </summary>
        /// <param name="debug">0:release, 1:debug</param>
        static void Main(int version = 0, int debug = 0) {
            UpdatorDto updatorDto = null;
            using var svc = new ServiceExecutorManager<IUpdatorExistsService>(new UpdatorExistsService());
            svc.SetRequest(o=> o.Request = version)
                .OnExecuted(o => {
                    updatorDto = o.Result;
                });

            if (updatorDto.jIsNotNull()) {
                using var updatorSvc = new ServiceExecutorManager<IUpdatorService>(new UpdatorService());
                updatorSvc.AddFilter(o => o.Result.jIsNotNull())
                    .SetRequest(o => o.Request = updatorDto.FileList)
                    .OnExecuted(o => {
                        if (o.Result) {
                            Console.WriteLine("downloaded");
                        }
                    });
            }
        }
    }
}
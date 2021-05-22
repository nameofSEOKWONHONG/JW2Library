using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;
using TodoWebApi.Services;

namespace TodoWebApi {
    public static class ServiceLoader {
        private static readonly IEnumerable<IServiceRegister> _serviceRegisters = new XList<IServiceRegister> {
            new TodoSvcRegister()
        };

        public static void ServiceLoad(this IServiceCollection services) {
            _serviceRegisters.xForEach(item => { item.ServiceRegistry(services); });
        }
    }
}
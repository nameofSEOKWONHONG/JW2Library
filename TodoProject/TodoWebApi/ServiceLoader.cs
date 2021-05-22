using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;
using TodoWebApi.Services;

namespace TodoWebApi {
    public static class ServiceLoader {
        public static void Load(this IServiceCollection services) {
            //TODO : 동적으로 처리 가능한가? 확인하자.
            var serviceRegisters = new XList<IServiceRegister> {
                new TodoSvcRegister()
            };
            serviceRegisters.xForEach(item => {
                item.ServiceRegistry(services);
            });
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using eXtensionSharp;
using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.Web {
    public static class ServiceLoader {
        public static void Load(this IServiceCollection services, IEnumerable<IServiceRegister> serviceRegisters) {
            //TODO : 동적으로 처리 가능한가? 확인하자.
            serviceRegisters.xForEach(item => {
                item.ServiceRegistry(services);
            });
        }
    }
}
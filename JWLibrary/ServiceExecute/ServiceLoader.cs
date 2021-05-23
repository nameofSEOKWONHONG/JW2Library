using System;
using System.Collections.Generic;
using System.Linq;
using eXtensionSharp;
using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.ServiceExecutor {
    /// <summary>
    ///     ServiceRegister를 등록한다.
    ///     둘중 하나만 사용하라.
    /// </summary>
    public static class ServiceLoader {
        /// <summary>
        ///     명시적으로 선언하여 진행함.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceRegisters"></param>
        public static void SvcLoad(this IServiceCollection services, IEnumerable<IServiceRegister> serviceRegisters) {
            serviceRegisters.xForEach(item => { item.ServiceRegistry(services); });
        }

        /// <summary>
        ///     동적 인스턴스 객체를 생성하여 진행함.
        /// </summary>
        /// <param name="services"></param>
        public static void SvcLoad(this IServiceCollection services) {
            var serviceRegisters = new XList<IServiceRegister>();
            AppDomain.CurrentDomain.GetAssemblies().xForEach(assembly => {
                var types = assembly.GetTypes()
                    .Where(m => m.FullName.Contains("ServiceRegister") && m.IsClass);

                types.xForEach(type => { serviceRegisters.Add(Activator.CreateInstance(type) as IServiceRegister); });
            });

            if (serviceRegisters.xIsNotEmpty())
                serviceRegisters.xForEach(svcRegister => { svcRegister.ServiceRegistry(services); });
        }
    }
}
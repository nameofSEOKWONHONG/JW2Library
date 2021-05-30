using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static void SvcLoad(this IServiceCollection services, IEnumerable<IServiceInjector> serviceRegisters) {
            serviceRegisters.xForEach(item => { item.Register(services); });
        }

        /// <summary>
        ///     동적 인스턴스 객체를 생성하여 진행함.
        /// </summary>
        /// <param name="services"></param>
        public static void SvcLoad(this IServiceCollection services) {
            var serviceRegisters = new XList<IServiceInjector>();

            foreach (string assemblyPath in Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories))
            {
                if (assemblyPath.Contains("ServiceInjector")) {
                    var assembly = Assembly.LoadFile(assemblyPath);
                    try {
                        var types = assembly.GetTypes();
                        types.xForEach(type => {
                            if (type.FullName.Contains("ServiceInjector") && type.IsClass) {
                                serviceRegisters.Add(Activator.CreateInstance(type) as IServiceInjector);
                            }
                        });
                    }
                    catch {
                        //why exception? system.xaml   
                    }   
                }
            }

            if (serviceRegisters.xIsNotEmpty())
                serviceRegisters.xForEach(svcRegister => {
                    svcRegister.Register(services);
                });
        }
    }
}
using System.Collections.Generic;
using JWLibrary.Core;
using Microsoft.Extensions.DependencyInjection;
using Service;
using Service.Accounts;
using Service.Config;
using Service.WeatherForecast;

namespace APIServer.Config {
    public static class ServiceLoader {
        private static readonly IEnumerable<IServiceRegister> _serviceRegisters = new JList<IServiceRegister>() {
            new AccountServiceRegister(),
            new ConfigServiceRegister(),
            new WeatherServiceRegister()
        };        
        public static void ServiceLoad(this IServiceCollection services) {
            _serviceRegisters.jForeach(item => {
                item.ServiceRegistry(services);
            });
        }
    }
}
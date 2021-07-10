using System;
using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.DI
{
    /// <summary>
    ///     서비스 로케이터
    ///     사용자 지정 DI를 수행합니다.(남용하지 마세요...)
    /// </summary>
    public class ServiceLocator
    {
        private static ServiceProvider _serviceProvider;
        private readonly ServiceProvider _currentServiceProvider;

        public ServiceLocator(ServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ServiceLocator Current => new(_serviceProvider);

        public static void SetLocatorProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return _currentServiceProvider.GetService<TService>();
        }
    }
}
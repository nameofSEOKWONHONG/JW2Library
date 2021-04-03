using Microsoft.Extensions.DependencyInjection;

namespace Service.Config {
    public static class ServiceLoader {
        public static void ConfigServiceLoader(this IServiceCollection serviceCollection) {
            serviceCollection.AddScoped<IGetConfigSvc, GetConfigSvc>();
        }
    }
}
using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Config {
    public class ConfigServiceRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetConfigSvc, GetConfigSvc>();
            services.AddScoped<IGetListConfigSvc, GetListConfigSvc>();
            services.AddScoped<ISaveConfigSvc, SaveConfigSvc>();
            services.AddScoped<IBulkSaveConfigSvc, BulkSaveConfigSvc>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Service.Data;

namespace Service.Accounts {
    public static class ServiceLoader {
        public static void AccountServiceLoader(this IServiceCollection services) {
            services.AddScoped<IGetAccountSvc, GetAccountSvc>();
            services.AddScoped<IGetAccountsSvc, GetAccountsSvc>();
            services.AddScoped<IDeleteAccountSvc, DeleteAccountSvc>();
            services.AddScoped<ISaveAccountSvc, SaveAccountSvc>();
            services.AddScoped<IGetAccountByIdSvc, GetAccountByIdSvc>();
        }
    }
}
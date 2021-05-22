using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;
using Service.Data;

namespace Service.Accounts {
    public class AccountServiceRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetAccountSvc, GetAccountSvc>();
            services.AddScoped<IGetAccountsSvc, GetAccountsSvc>();
            services.AddScoped<IDeleteAccountSvc, DeleteAccountSvc>();
            services.AddScoped<ISaveAccountSvc, SaveAccountSvc>();
            services.AddScoped<IGetAccountByIdSvc, GetAccountByIdSvc>();
        }
    }
}
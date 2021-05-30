using AccountService.Svc;
using JWLibrary.ServiceExecutor;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService {
    
    /// <summary>
    /// account service register
    /// </summary>
    public class AccountServiceInjector : IServiceInjector {
        public void Register(IServiceCollection services) {
            services.AddScoped<IGetAccountSvc, GetAccountSvc>();
            services.AddScoped<IGetAccountsSvc, GetAccountsSvc>();
            services.AddTransient<ISaveAccountSvc, SaveAccountSvc>();
            services.AddTransient<IDeleteAccountSvc, DeleteAccountSvc>();
        }
    }
}
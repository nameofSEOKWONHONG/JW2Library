using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;

namespace TodoWebApi.Services {
    public class TodoSvcRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetTodoItemSvc, GetTodoItemSvc>();
            services.AddScoped<IGetTodoItemsSvc, GetTodoItemsSvc>();
        }
    }
}
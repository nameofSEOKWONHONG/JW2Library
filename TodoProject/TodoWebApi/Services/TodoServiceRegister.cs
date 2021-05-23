using JWLibrary.ServiceExecutor;
using Microsoft.Extensions.DependencyInjection;

namespace TodoWebApi.Services {
    /// <summary>
    ///     서비스 등록자
    /// </summary>
    public class TodoServiceRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetTodoItemSvc, GetTodoItemSvc>();
            services.AddScoped<IGetTodoItemsSvc, GetTodoItemsSvc>();
            services.AddTransient<ISaveTodoItemSvc, SaveTodoItemSvc>();
            services.AddTransient<IDeleteTodoItemSvc, DeleteTodoItemSvc>();
        }
    }
}
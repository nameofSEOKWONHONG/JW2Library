using JWLibrary.Web;
using Microsoft.Extensions.DependencyInjection;

namespace TodoWebApi.Services {
    /// <summary>
    /// 서비스 등록자
    /// </summary>
    public class TodoSvcRegister : IServiceRegister {
        public void ServiceRegistry(IServiceCollection services) {
            services.AddScoped<IGetTodoItemSvc, GetTodoItemSvc>();
            services.AddScoped<IGetTodoItemsSvc, GetTodoItemsSvc>();
            services.AddTransient<ISaveTodoItemSvc, SaveTodoItemSvc>();
            services.AddTransient<IDeleteTodoItemSvc, DeleteTodoItemSvc>();
        }
    }
}
using JWLibrary.ServiceExecutor;
using Microsoft.Extensions.DependencyInjection;

namespace TodoService {
    /// <summary>
    ///     서비스 등록자
    /// </summary>
    public class TodoServiceInjector : IServiceInjector {
        public void Register(IServiceCollection services) {
            services.AddScoped<IGetTodoItemSvc, GetTodoItemSvc>();
            services.AddScoped<IGetTodoItemsSvc, GetTodoItemsSvc>();
            services.AddTransient<ISaveTodoItemSvc, SaveTodoItemSvc>();
            services.AddTransient<IDeleteTodoItemSvc, DeleteTodoItemSvc>();
            services.AddTransient<ITransactionSampleSvc, TransactionSampleSvc>();
        }
    }
}
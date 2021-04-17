using Microsoft.Extensions.DependencyInjection;

namespace Service {
    public interface IServiceRegister {
        void ServiceRegistry(IServiceCollection services);
    }
}
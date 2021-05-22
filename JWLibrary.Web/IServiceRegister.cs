using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.Web {
    public interface IServiceRegister {
        void ServiceRegistry(IServiceCollection services);
    }
}
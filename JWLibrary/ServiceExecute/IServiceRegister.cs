using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.ServiceExecutor {
    /// <summary>
    /// 서비스 레지스터 인터페이스
    /// </summary>
    public interface IServiceRegister {
        void ServiceRegistry(IServiceCollection services);
    }
}
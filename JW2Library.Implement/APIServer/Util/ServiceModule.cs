using Autofac;

namespace APIServer.Util {
    /// <summary>
    ///     autofac service register(don't remove)
    /// </summary>
    public class ServiceModule : Module {
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);
            //builder.RegisterType<IHelloService>.As<HelloService>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace JWLibrary.ApiCore.Util {
    
    /// <summary>
    /// autofac service register(don't remove)
    /// </summary>
    public class ServiceModule : Module {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //builder.RegisterType<IHelloService>.As<HelloService>();
        }
    }
}

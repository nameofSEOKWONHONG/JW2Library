using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GrpcService {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.ConfigureKestrel(options => {
                        //options.Listen(IPAddress.Any, 5001, listenOptions =>
                        //{
                        //    listenOptions.Protocols = HttpProtocols.Http2;
                        //    listenOptions.UseConnectionLogging("Connection");
                        //    listenOptions.KestrelServerOptions.ListenLocalhost(5000, config => {
                        //        config.Protocols = HttpProtocols.Http2;
                        //    });
                        //});

                        // Setup a HTTP/2 endpoint without TLS.
                        //options.ListenLocalhost(5000, o => o.Protocols =
                        //    HttpProtocols.Http2);
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
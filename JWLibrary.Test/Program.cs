using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace JCoreSvcTest {
    class Program {
        static void Main(string[] args) {
            ITestService service = new TestService();

            var result = string.Empty;
            using (var executor = new ServiceExecutorManager<ITestService>(service)) {
                executor.SetRequest(o => o.Request = "Service Executor ")
                    .AddFilter(o => o.Request.Length > 0)
                    .AddFilter(o => o.Request.jIsNotNull())
                    .OnExecuted(o => {
                        result = o.Result;
                    });
            }
        }
    }

    public interface ITestService : IServiceExecutor<string, string> { }

    public class TestService : ServiceExecutor<TestService, string, string>, ITestService {
        public TestService() {
            base.SetValidator(new TestServiceValidator());
        }

        public override void Execute() {
            this.Result = $"{this.Request}_Hello_World";
        }

        public class TestServiceValidator : AbstractValidator<TestService> {
            public TestServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}

using System;
using System.Net.Http;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace JWUpdator {
    public class UpdatorDto {
        public int Version { get; set; }
        public JLKList<string> FileList { get; set; }
    }

    public interface IUpdatorExistsService : IServiceExecutor<int, UpdatorDto> {
    }

    public class UpdatorExistsService : ServiceExecutor<UpdatorExistsService, int, UpdatorDto>, IUpdatorExistsService {
        private readonly string _baseUrl = "https://raw.githubusercontent.com";
        private readonly string _updatorUrl = "/DEV-TEAM-RED/redconfigs/main/updatorconfig.json";
        private UpdatorDto _updatorDto;

        public UpdatorExistsService() {
            base.SetValidator(new Validator());
        }

        public override bool PreExecute() {
            var nowVersion = 0;
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var result = client.GetAsync(_updatorUrl).GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode) {
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                _updatorDto = content.jToObject<UpdatorDto>();
            }

            return true;
        }

        public override void Execute() {
            if (Request < _updatorDto.Version) Result = _updatorDto;
        }

        public class Validator : AbstractValidator<UpdatorExistsService> {
            public Validator() {
                RuleFor(m => m.Request).GreaterThanOrEqualTo(0);
            }
        }
    }
}
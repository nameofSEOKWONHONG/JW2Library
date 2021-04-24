using System;
using System.Collections.Generic;
using System.Net.Http;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.ServiceExecutor;

namespace JWUpdator {
    /// <summary>
    ///     update dto, 필요한 설정정보 dto
    /// </summary>
    public class UpdatorDto {
        /// <summary>
        ///     version no
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        ///     update file list
        /// </summary>
        public IEnumerable<string> FileList { get; set; }
    }

    /// <summary>
    ///     UpdatorExistsService interface
    /// </summary>
    public interface IUpdatorExistsService : IServiceExecutor<int, UpdatorDto> {
    }

    /// <summary>
    ///     IUpdatorExistsService implement
    /// </summary>
    public class UpdatorExistsService : ServiceExecutor<UpdatorExistsService, int, UpdatorDto>, IUpdatorExistsService {
        private readonly string _baseUrl = "https://raw.githubusercontent.com";
        private readonly string _updatorUrl = "/DEV-TEAM-RED/redconfigs/main/updatorconfig.json";
        private UpdatorDto _updatorDto;

        /// <summary>
        ///     creator
        /// </summary>
        public UpdatorExistsService() {
            base.SetValidator(new Validator());
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override bool PreExecute() {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var result = client.GetAsync(_updatorUrl).GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode) {
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                _updatorDto = content.xFromJsonToObject<UpdatorDto>();
            }

            return true;
        }

        /// <summary>
        /// </summary>
        public override void Execute() {
            if (Request < _updatorDto.Version) Result = _updatorDto;
        }

        /// <summary>
        ///     validator
        /// </summary>
        public class Validator : AbstractValidator<UpdatorExistsService> {
            /// <summary>
            /// </summary>
            public Validator() {
                RuleFor(m => m.Request).GreaterThanOrEqualTo(0);
            }
        }
    }
}
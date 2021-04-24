using System;
using System.Collections;
using System.Collections.Generic;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.ServiceExecutor;
using JWLibrary.Utils;
using JWLibrary.Utils.Files;

namespace JWUpdator {
    /// <summary>
    ///     update interface
    /// </summary>
    public interface IUpdatorService : IServiceExecutor<IEnumerable<string>, bool> {
    }

    /// <summary>
    ///     update service
    /// </summary>
    public class UpdatorService : ServiceExecutor<UpdatorService, IEnumerable<string>, bool>, IUpdatorService {
        private readonly string _baseUrl = "https://raw.githubusercontent.com";
        private readonly string _prefixUrl = "/DEV-TEAM-RED/redconfigs/main/";
        private readonly string _unzipPath = "download_" + DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        ///     ctor
        /// </summary>
        public UpdatorService() {
            base.SetValidator(new Validator());
        }

        /// <summary>
        ///     exeute
        /// </summary>
        public override void Execute() {
            Request.xForEach(file => {
                var downloadFile = $"{_prefixUrl}{file}";
                var localFileName = $"{"".xToPath()}{file}";
                var request = new JHttpRequest(_baseUrl);
                request.DownloadAsync(downloadFile, localFileName).GetAwaiter().GetResult();

                if (localFileName.jFileExists()) localFileName.jFileUnzip($"{"".xToPath()}{_unzipPath}");
                return true;
            });

            Result = true;
        }

        /// <summary>
        ///     validator
        /// </summary>
        public class Validator : AbstractValidator<UpdatorService> {
            /// <summary>
            ///     ctor
            /// </summary>
            public Validator() {
                RuleFor(m => m.Request).NotNull();
            }
        }
    }
}
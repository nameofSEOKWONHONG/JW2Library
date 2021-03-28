using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Utils;
using JWLibrary.Utils.Files;

namespace JWUpdator {
    public interface IUpdatorService : IServiceExecutor<JLKList<string>, bool> {
    }

    public class UpdatorService : ServiceExecutor<UpdatorService, JLKList<string>, bool>, IUpdatorService {
        private readonly string _baseUrl = "https://raw.githubusercontent.com";
        private readonly string _prefixUrl = "/DEV-TEAM-RED/redconfigs/main/";
        private readonly string _unzipPath = "download_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        
        public UpdatorService() {
            base.SetValidator(new Validator());
        }

        public override void Execute() {
            this.Request.jForEach(file => {
                var downloadFile = $"{this._prefixUrl}{file}";
                var localFileName = $"{"".jToAppPath()}{file}";
                var request = new HttpRequest(_baseUrl);
                request.DownloadAsync(downloadFile, localFileName).GetAwaiter().GetResult();

                if (localFileName.jFileExists()) {
                    localFileName.jFileUnzip($"{"".jToAppPath()}{_unzipPath}");
                }
                return true;
            });

            this.Result = true;
        }
        
        public class Validator : AbstractValidator<UpdatorService> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
            }
        }
    }
}
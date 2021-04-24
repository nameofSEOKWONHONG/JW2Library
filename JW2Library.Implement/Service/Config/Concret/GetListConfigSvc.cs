using System.Collections.Generic;
using eXtensionSharp;
using FluentValidation;

namespace Service.Config {
    public class GetListConfigSvc :
        ConfigSvcBase<GetListConfigSvc, IEnumerable<GetConfigRequest>, IEnumerable<GetConfigResult>>,
        IGetListConfigSvc {
        public GetListConfigSvc() {
            SetValidator<Validator>();
        }

        public override void Execute() {
            var results = new XList<GetConfigResult>();
            Request.xForEach(item => {
                var doc = Collection.FindOne($"$.key='{item.Key}'");
                results.Add(new GetConfigResult {
                    Key = item.Key,
                    Content = doc["value"].AsString
                });
            });
            Result = results;
        }

        public class Validator : AbstractValidator<GetListConfigSvc> {
            public Validator() {
                RuleForEach(m => m.Request).NotNull().NotEmpty();
            }
        }
    }
}
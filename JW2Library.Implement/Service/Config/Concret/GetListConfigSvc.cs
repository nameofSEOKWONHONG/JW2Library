using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using JWLibrary.Core;
using NetFabric.Hyperlinq;

namespace Service.Config {
    public class GetListConfigSvc : ConfigSvcBase<GetListConfigSvc, IEnumerable<GetConfigRequest>, IEnumerable<GetConfigResult>>, IGetListConfigSvc {
        public GetListConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            JList<GetConfigResult> results = new JList<GetConfigResult>();
            this.Request.jForEach(item => {
                var doc = this.Collection.FindOne($"$.key='{item.Key}'");
                results.Add(new GetConfigResult() {
                    Key = item.Key,
                    Content = doc["value"].AsString,
                });
            });
            this.Result = results;
        }

        public class Validator : AbstractValidator<GetListConfigSvc> {
            public Validator() {
                RuleForEach(m => m.Request).NotNull().NotEmpty();
            }
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using JWLibrary.Core;

namespace Service.Config {
    public class GetListConfigSvc : ConfigSvcBase<GetListConfigSvc, IEnumerable<string>, IEnumerable<string>>, IGetListConfigSvc {
        public GetListConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            JLKList<string> list = new JLKList<string>();
            this.Request.jForEach(item => {
                var doc = this.Collection.FindOne(item);
                list.Add(doc["value"].AsString);
            });

            this.Result = list;
        }

        public class Validator : AbstractValidator<GetListConfigSvc> {
            public Validator() {
                RuleForEach(m => m.Request).NotNull().NotEmpty();
            }
        }
        
    }
}
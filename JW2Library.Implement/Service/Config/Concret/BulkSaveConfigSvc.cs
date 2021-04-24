using System.Collections.Generic;
using eXtensionSharp;
using FluentValidation;
using JWLibrary.ServiceExecutor;

namespace Service.Config {
    public class BulkSaveConfigSvc :
        ConfigSvcBase<BulkSaveConfigSvc, IEnumerable<SaveConfigRequest>, IEnumerable<SaveConfigResult>>,
        IBulkSaveConfigSvc {
        public BulkSaveConfigSvc() {
            SetValidator<Validator>();
        }

        public override void Execute() {
            using (var svc = new BulkServiceExecutorManager<SaveConfigSvc, SaveConfigRequest>(Request)) {
                svc.SetRequest((o, current) => { o.Request = current; })
                    .AddFilter(o => { return o.Request.xIsNotNull(); }).OnExecuted(o => {
                        var result = o.Result;
                        return true;
                    });
            }
        }

        public class Validator : AbstractValidator<BulkSaveConfigSvc> {
            public Validator() {
                RuleForEach(m => m.Request).NotNull();
                RuleForEach(m => m.Request).SetValidator(new SaveConfigRequest.Validator());
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;

namespace Service.Config {
    public class BulkSaveConfigSvc : ConfigSvcBase<BulkSaveConfigSvc, IEnumerable<string>, Tuple<bool, IEnumerable<string>>>, IBulkSaveConfigSvc {
        public BulkSaveConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            using (var svc = new BulkServiceExecutorManager<SaveConfigSvc, string>(this.Request)) {
                svc.SetRequest((o, current) => {
                    o.Request = current;
                }).AddFilter(o => {
                    return o.Request.jIsNotNull();
                }).OnExecuted(o => {
                    var result = o.Result;
                });
            }
        }

        public class Validator : AbstractValidator<BulkSaveConfigSvc> {
            public Validator() {
                RuleForEach(m => m.Request).NotNull().NotEmpty();
            }
        }
    }
}
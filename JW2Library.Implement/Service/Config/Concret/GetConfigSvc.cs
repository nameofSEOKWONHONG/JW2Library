using System;
using eXtensionSharp;
using FluentValidation;

namespace Service.Config {
    public class GetConfigSvc : ConfigSvcBase<GetConfigSvc, GetConfigRequest, GetConfigResult>, IGetConfigSvc {
        public GetConfigSvc() {
            SetValidator<Validator>();
        }

        public override void Execute() {
            var bsonDocument = Collection.FindOne($"$.key='{Request.Key}'");
            Result = new GetConfigResult {
                Key = Request.Key,
                Content = bsonDocument["value"].AsString
            };
        }

        public override void PostExecute() {
            if (Result.Content.xIsNullOrEmpty()) throw new Exception("key is empty.");
            base.PostExecute();
        }

        public class Validator : AbstractValidator<GetConfigSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
                RuleFor(m => m.Request.Key).NotNull().NotEmpty();
            }
        }
    }
}
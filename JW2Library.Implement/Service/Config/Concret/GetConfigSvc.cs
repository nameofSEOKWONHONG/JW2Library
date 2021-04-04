using System;
using System.Collections.Generic;
using System.IO;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using LiteDB;
using LiteDbFlex;
using Service.Data;

namespace Service.Config {
    public class GetConfigSvc : ConfigSvcBase<GetConfigSvc, GetConfigRequest, GetConfigResult>, IGetConfigSvc {
        public GetConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            var bsonDocument = this.Collection.FindOne($"$.key='{this.Request.Key}'");
            this.Result = new GetConfigResult() {
                Key = this.Request.Key,
                Content = bsonDocument["value"].AsString
            };
        }

        public override void PostExecute() {
            if (this.Result.Content.jIsNullOrEmpty()) throw new Exception("key is empty.");
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
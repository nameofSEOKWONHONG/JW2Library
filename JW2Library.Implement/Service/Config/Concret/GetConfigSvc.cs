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
    public class GetConfigSvc : ConfigSvcBase<GetConfigSvc, string, string>, IGetConfigSvc {
        public GetConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            var bsonDocument = this.Collection.FindOne(this.Request);
            this.Result = bsonDocument["value"].AsString;
        }

        public override void PostExecute() {
            if (this.Result.jIsNullOrEmpty()) throw new Exception("key is empty.");
            base.PostExecute();
        }

        public class Validator : AbstractValidator<GetConfigSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
            }
        }
    }
}
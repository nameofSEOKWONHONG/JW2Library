using System;
using FluentValidation;
using JWLibrary.Core;
using LiteDB;

namespace Service.Config {
    public class SaveConfigSvc : ConfigSvcBase<SaveConfigSvc, string, Tuple<bool, string>>, ISaveConfigSvc{
        public SaveConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            var key = this.Request.GetHashCode().ToString();
            var doc = new BsonDocument();
            doc["key"] = key;
            doc["value"] = this.Request;
            this.Collection.Insert(doc);
            this.Result = new Tuple<bool, string>(key.jIsNullOrEmpty(), key);
        }

        public class Validator : AbstractValidator<SaveConfigSvc> {
            public Validator() {
                RuleFor(m => m.Request).NotNull();
            }
        }
    }
}
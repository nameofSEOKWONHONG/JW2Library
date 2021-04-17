using System;
using FluentValidation;
using JWLibrary.Core;
using LiteDB;

namespace Service.Config {
    public class SaveConfigSvc : ConfigSvcBase<SaveConfigSvc, SaveConfigRequest, SaveConfigResult>, ISaveConfigSvc{
        public SaveConfigSvc() {
            this.SetValidator<Validator>();
        }

        public override void Execute() {
            var key = this.Request.Content.GetHashCode().ToString();
            var doc = new BsonDocument();
            doc["key"] = key;
            doc["value"] = this.Request.Content;
            doc["write_dt"] = this.Request.WriteDt;
            doc["user_id"] = this.Request.UserId;
            var bsonValue = this.Collection.Insert(doc);
            this.Result = new SaveConfigResult() {
                IsSuccess = bsonValue.AsObjectId.Pid > 0,
                Key = key
            };
        }

        public class Validator : AbstractValidator<SaveConfigSvc> {
            public Validator() {
                RuleFor(m => m.Request).SetValidator(new SaveConfigRequest.Validator());
            }
        }
    }
}
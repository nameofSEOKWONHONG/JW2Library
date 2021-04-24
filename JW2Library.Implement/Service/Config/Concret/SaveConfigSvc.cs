using FluentValidation;
using LiteDB;

namespace Service.Config {
    public class SaveConfigSvc : ConfigSvcBase<SaveConfigSvc, SaveConfigRequest, SaveConfigResult>, ISaveConfigSvc {
        public SaveConfigSvc() {
            SetValidator<Validator>();
        }

        public override void Execute() {
            var key = Request.Content.GetHashCode().ToString();
            var doc = new BsonDocument();
            doc["key"] = key;
            doc["value"] = Request.Content;
            doc["write_dt"] = Request.WriteDt;
            doc["user_id"] = Request.UserId;
            var bsonValue = Collection.Insert(doc);
            Result = new SaveConfigResult {
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
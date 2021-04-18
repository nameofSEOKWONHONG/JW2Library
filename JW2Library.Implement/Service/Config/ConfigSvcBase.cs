using System.IO;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using LiteDB;

namespace Service.Config {
    public class ConfigSvcBase<TOwner, TRequest, TResult> : ServiceExecutor<TOwner, TRequest, TResult>
        where TOwner : ConfigSvcBase<TOwner, TRequest, TResult> {
        protected LiteDatabase LiteDatabase { get; private set; }
        protected readonly ILiteCollection<BsonDocument> Collection; 
        public ConfigSvcBase() {
            var dir = ConfigConst.PRE_CONFIG_DIR.jToPath();
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            var connectionString =
                new LiteDB.ConnectionString($"{ConfigConst.PRE_CONFIG_DIR}/{ConfigConst.CONFIG_DB_FILE_NAME}") {
                    Connection = ConnectionType.Shared
                };
            if(this.LiteDatabase.jIsNull())
                this.LiteDatabase = new LiteDatabase(connectionString);
            
            if(this.Collection.jIsNull())
                this.Collection = LiteDatabase.GetCollection(ConfigConst.DB_NAME);
        }

        public override void Dispose() {
            this.LiteDatabase.Dispose();
        }
    }
}
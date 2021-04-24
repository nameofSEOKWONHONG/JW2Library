using System.IO;
using eXtensionSharp;
using JWLibrary.ServiceExecutor;
using LiteDB;

namespace Service.Config {
    public class ConfigSvcBase<TOwner, TRequest, TResult> : ServiceExecutor<TOwner, TRequest, TResult>
        where TOwner : ConfigSvcBase<TOwner, TRequest, TResult> {
        protected readonly ILiteCollection<BsonDocument> Collection;

        public ConfigSvcBase() {
            var dir = ConfigConst.PRE_CONFIG_DIR.xToPath();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var connectionString =
                new ConnectionString($"{ConfigConst.PRE_CONFIG_DIR}/{ConfigConst.CONFIG_DB_FILE_NAME}") {
                    Connection = ConnectionType.Shared
                };
            if (LiteDatabase.xIsNull())
                LiteDatabase = new LiteDatabase(connectionString);

            if (Collection.xIsNull())
                Collection = LiteDatabase.GetCollection(ConfigConst.DB_NAME);
        }

        protected LiteDatabase LiteDatabase { get; }

        public override void Dispose() {
            LiteDatabase.Dispose();
        }
    }
}
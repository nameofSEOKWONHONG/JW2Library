using eXtensionSharp;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace JWLibrary.Database
{
    public static class MongoExtension
    {
        public static TEntity xToEntity<TEntity>(this BsonDocument doc) where TEntity : class, new()
        {
            if (doc.xIsNotEmpty()) return BsonSerializer.Deserialize<TEntity>(doc);

            return new TEntity();
        }
    }
}
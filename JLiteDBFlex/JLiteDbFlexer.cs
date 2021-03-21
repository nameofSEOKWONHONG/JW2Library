using LiteDB;

namespace JLiteDBFlex {
    public interface IJLiteDbFlexer {
        ILiteDatabase LiteDatabase { get; }
    }

    public class JLiteDbFlexer<T> : IJLiteDbFlexer
        where T : class {
        public JLiteDbFlexer(string additionalDbFileName = "") {
            var resolveInfo = JLiteDbResolver.Resolve<T>(additionalDbFileName);

            TableName = resolveInfo.tableName;
            FileName = resolveInfo.fileName;
            LiteDatabase = resolveInfo.liteDatabase;

            LiteCollection = resolveInfo.liteDatabase.GetCollection<T>(resolveInfo.tableName);
            foreach (var index in resolveInfo.indexItems) LiteCollection.EnsureIndex(index.Key, index.Value);
        }

        public ILiteCollection<T> LiteCollection { get; }
        public string TableName { get; }
        public string FileName { get; }
        public ILiteDatabase LiteDatabase { get; }
    }
}
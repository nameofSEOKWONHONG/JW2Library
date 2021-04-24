using eXtensionSharp;
using LiteDB;

namespace JLiteDBFlex {
    internal interface ILiteDbFlexer {
        ILiteDatabase LiteDatabase { get; }
    }

    internal class LiteDbFlexer<T> : ILiteDbFlexer
        where T : class {
        #region [property]
        public ILiteCollection<T> LiteCollection { get; private set; }
        public string TableName { get; private set; }
        public string FileName { get; private set; }
        public ILiteDatabase LiteDatabase { get; private set; }
        #endregion

        #region [ctor]
        public LiteDbFlexer() {
            var resolveInfo = LiteDbResolver.Resolve<T>();

            TableName = resolveInfo.tableName;
            FileName = resolveInfo.fileName;
            LiteDatabase = resolveInfo.liteDatabase;

            LiteCollection = resolveInfo.liteDatabase.GetCollection<T>(resolveInfo.tableName);
            resolveInfo.indexItems.xForEach(indexItem => {
                LiteCollection.EnsureIndex(indexItem.Key, indexItem.Value);
            }); 
        }
        #endregion
    }
}
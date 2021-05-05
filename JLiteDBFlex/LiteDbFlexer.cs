using System.Collections.Generic;
using eXtensionSharp;
using LiteDB;

namespace JLiteDBFlex {
    public interface ILiteDbFlexer {
        string TableName { get; }
        string FileName { get; }
        ILiteDatabase LiteDatabase { get; }
    }

    internal class LiteDbFlexer<T> : ILiteDbFlexer where T : class {
        #region [property]
        public string TableName { get; private set; }
        public string FileName { get; private set; }
        public ILiteDatabase LiteDatabase { get; private set; }
        #endregion

        #region [ctor]
        public LiteDbFlexer(ConnectionType type = ConnectionType.Shared) {
            var resolveInfo = LiteDbResolver.Resolve<T>(type);

            TableName = resolveInfo.tableName;
            FileName = resolveInfo.fileName;
            LiteDatabase = resolveInfo.liteDatabase;
        }
        #endregion
    }

    internal class LiteDbFlexer : ILiteDbFlexer {
        #region [property]
        public string TableName { get; private set; }
        public string FileName { get; private set; }
        public ILiteDatabase LiteDatabase { get; private set; }
        #endregion

        #region [ctor]
        public LiteDbFlexer(string fileName, string tableName, Dictionary<string, bool> indexItems = null, ConnectionType type = ConnectionType.Shared) {
            var resolveInfo = LiteDbResolver.Resolve(fileName, tableName, indexItems, type);

            TableName = resolveInfo.tableName;
            FileName = resolveInfo.fileName;
            LiteDatabase = resolveInfo.liteDatabase;
        }
        #endregion
    }
}
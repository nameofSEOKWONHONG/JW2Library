using System;
using System.Linq;
using C5;
using JWLibrary.Core;
using LiteDB;

namespace JLiteDBFlex {
    public class JLiteDbFlexer<T>
    where T : class {
        public ILiteDatabase LiteDatabase { get; }
        public ILiteCollection<T> LiteCollection { get; }
        public string TableName { get; }
        public string FileName { get; }
        
        public JLiteDbFlexer(string additionalDbFileName = "") {
            var resolveInfo = JLiteDbResolver.Resolve<T>(additionalDbFileName);

            this.TableName = resolveInfo.tableName;
            this.FileName = resolveInfo.fileName;
            this.LiteDatabase = resolveInfo.liteDatabase;
            
            LiteCollection = resolveInfo.liteDatabase.GetCollection<T>(resolveInfo.tableName);
            foreach (var index in resolveInfo.indexItems) {
                LiteCollection.EnsureIndex(index.Key, index.Value);
            }
        }
    }
}
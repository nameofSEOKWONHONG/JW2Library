using System.Collections.Generic;
using eXtensionSharp;
using LiteDB;

namespace JLiteDBFlex {
    /// <summary>
    ///     create litedb instance
    /// </summary>
    internal class LiteDbResolver {
        private LiteDbResolver() {
        }

        public static (ILiteDatabase liteDatabase, string fileName, string tableName, Dictionary<string, bool> indexItems) Resolve<TEntity>()
            where TEntity : class {
            var fileName =
                typeof(TEntity).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.FileName);
            
            var tableName =
                typeof(TEntity).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.TableName);
            
            var indexItems =
                typeof(TEntity).xGetAttrValue((LiteDbTableAttribute indexAttribute) => indexAttribute.IndexItems);

            var conStr = new ConnectionString() {
                Filename = fileName,
                Connection = ConnectionType.Shared,
            };

            var database = new LiteDatabase(conStr);
            
            return (database, fileName, tableName, indexItems);
        }
    }
}
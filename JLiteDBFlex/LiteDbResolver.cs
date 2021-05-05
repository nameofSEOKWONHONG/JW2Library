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

        public static (ILiteDatabase liteDatabase, string fileName, string tableName) 
            Resolve<TEntity>(ConnectionType type = ConnectionType.Shared)
            where TEntity : class {
            var fileName =
                typeof(TEntity).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.FileName);
            
            var tableName =
                typeof(TEntity).xGetAttrValue((LiteDbTableAttribute tableAttribute) => tableAttribute.TableName);
            
            var conStr = new ConnectionString() {
                Filename = fileName,
                Connection = type,
            };

            var database = new LiteDatabase(conStr);
            
            return (database, fileName, tableName);
        }

        public static (ILiteDatabase liteDatabase, string fileName, string tableName) 
            Resolve(string fileName, string tableName, Dictionary<string, bool> indexItems = null, ConnectionType type = ConnectionType.Shared) {

            var conStr = new ConnectionString() {
                Filename = fileName,
                Connection = type
            };

            var database = new LiteDatabase(conStr);

            return (database, fileName, tableName);
        }
    }
}
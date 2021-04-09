using System.Collections.Generic;
using JWLibrary.Core;
using LiteDB;

namespace JLiteDBFlex {
    /// <summary>
    ///     create litedb instance
    /// </summary>
    internal class JLiteDbResolver {
        private JLiteDbResolver() {
        }

        public static (ILiteDatabase liteDatabase, string fileName, string tableName, Dictionary<string, bool>
            indexItems) Resolve<TEntity>(string additionalDbFileName = "")
            where TEntity : class {
            var fileName =
                typeof(TEntity).getAttrValue((JLiteDbTableAttribute tableAttribute) => tableAttribute.FileName);
            if (additionalDbFileName.isEmpty()) fileName = $"{additionalDbFileName}_{fileName}";

            var tableName =
                typeof(TEntity).getAttrValue((JLiteDbTableAttribute tableAttribute) => tableAttribute.TableName);
            var indexItems =
                typeof(TEntity).getAttrValue((JLiteDbTableAttribute indexAttribute) => indexAttribute.IndexItems);

            LiteDatabase database = null;

            if (!string.IsNullOrEmpty(fileName)) database = new LiteDatabase(fileName);

            return (database, fileName, tableName, indexItems);
        }
    }
}
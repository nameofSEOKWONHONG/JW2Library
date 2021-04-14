using System;
using System.Collections.Generic;

namespace JLiteDBFlex {
    /// <summary>
    ///     litedb entity extension attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LiteDbTableAttribute : Attribute {
        public string FileName { get; }
        public string TableName { get; }
        
        public readonly Dictionary<string, bool> IndexItems = new();
        
        public LiteDbTableAttribute(string fileName, string tableName, string[] indexNames = null) {
            FileName = fileName;
            TableName = tableName;
            if (indexNames != null)
                for (var i = 0; i < indexNames.Length; i++) {
                    IndexItems.Add(indexNames[i], true);
                }
        }
    }
}
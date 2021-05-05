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
        
        public LiteDbTableAttribute(string fileName, string tableName) {
            FileName = fileName;
            TableName = tableName;
        }
    }
}
using System;
using System.Collections.Generic;

namespace JLiteDBFlex {
    /// <summary>
    ///     litedb entity extension attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class JLiteDbTableAttribute : Attribute {
        public readonly Dictionary<string, bool> IndexItems = new();

        public JLiteDbTableAttribute(string fileName, string tableName, string[] indexNames = null,
            bool[] indexUniques = null) {
            FileName = fileName;
            TableName = tableName;
            if (indexNames != null)
                for (var i = 0; i < indexNames.Length; i++) {
                    var unique = true;
                    if (indexUniques != null) unique = indexUniques[i];
                    IndexItems.Add(indexNames[i], unique);
                }
        }

        public string FileName { get; }
        public string TableName { get; }
    }
}
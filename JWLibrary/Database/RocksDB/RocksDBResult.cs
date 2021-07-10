using System.Collections.Generic;

namespace JWLibrary.Database
{
    public class RocksDBResult
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }

        /// <summary>
        ///     true:success, false:failed
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        ///     state message
        /// </summary>
        public string StateMsg { get; set; }
    }

    public class RocksDBRequest
    {
        public string Path { get; set; }
        public ROCKSDB_COMMAND Command { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        /// <summary>
        ///     ROCKSDB_COMMAND = Gets
        /// </summary>
        public IEnumerable<string> Keys { get; set; }

        public Dictionary<string, string> KeyValues { get; set; }
    }
}
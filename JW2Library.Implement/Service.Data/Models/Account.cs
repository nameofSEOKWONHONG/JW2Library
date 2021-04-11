using JLiteDBFlex;

namespace JWService.Data.Models {
    [LiteDbTable("account.db", "accounts")]
    public class Account {
        /// <summary>
        ///     Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        ///     hashId
        /// </summary>
        /// <value></value>
        public string HashId { get; set; }

        /// <summary>
        ///     userId
        /// </summary>
        /// <value></value>
        public string UserId { get; set; }

        /// <summary>
        ///     password
        /// </summary>
        /// <value></value>
        public string Passwd { get; set; }
    }
}
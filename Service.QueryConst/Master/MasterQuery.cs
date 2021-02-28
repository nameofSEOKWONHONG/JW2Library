using System;
using JWLibrary.Core;
using JWLibrary.Utils.Files;

namespace Service.QueryConst {
    public class MasterQuery : QueryJSBase<MasterQuery> {
        public readonly string CREATE_DATABASE;
        public readonly string CREATE_TABLE;
        public readonly string CREATE_USER;
        public readonly string EXISTS_TABLE;

        public MasterQuery() {
            CREATE_DATABASE = this.ReadQueryJS("./Master/CREATE_DATABASE.js");
            CREATE_TABLE = this.ReadQueryJS("./Master/CREATE_TABLE.js");
            CREATE_USER = this.ReadQueryJS("./Master/CREATE_USER.js");
            EXISTS_TABLE = this.ReadQueryJS("./Master/EXISTS_TABLE.js");
        }
    }
}
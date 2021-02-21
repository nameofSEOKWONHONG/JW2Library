using System;
using JWLibrary.Core;
using JWLibrary.Utils.Files;

namespace Service.QueryConst.Master {
    public class MasterQueryFiles {
        private static Lazy<MasterQueryFiles> _instance = new Lazy<MasterQueryFiles>(() => new MasterQueryFiles());
        
        public static MasterQueryFiles Self {
            get { return _instance.Value; }
        }

        const string CARRIAGE_RETURN = "\n";
        public readonly string CREATE_DATABASE;
        public readonly string CREATE_TABLE;
        public readonly string CREATE_USER;
        public readonly string EXISTS_TABLE;

        public MasterQueryFiles() {
            CREATE_DATABASE = "./Master/CREATE_DATABASE.js".jReadLines().jJoin(CARRIAGE_RETURN);
            CREATE_TABLE    = "./Master/CREATE_TABLE.js".jReadLines().jJoin(CARRIAGE_RETURN);
            CREATE_USER     = "./Master/CREATE_USER.js".jReadLines().jJoin(CARRIAGE_RETURN);
            EXISTS_TABLE    = "./Master/EXISTS_TABLE.js".jReadLines().jJoin(CARRIAGE_RETURN);
        }
    }
}
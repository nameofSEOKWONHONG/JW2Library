using System;
using JWLibrary.Core;
using JWLibrary.Utils.Files;

namespace Service.QueryConst {
    public class QueryJSBase<T> 
        where T : class, new() {
        private const string CARRIAGE_RETURN = "\n";
        public static Lazy<T> _instance = new Lazy<T>(() => new T());

        public static T Self {
            get { return _instance.Value; }
        }

        protected string ReadQueryJS(string javascriptFile) {
            return javascriptFile.jReadLines().jJoin(CARRIAGE_RETURN);
        }
    }
}
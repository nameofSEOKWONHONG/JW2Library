using System;
using JWLibrary.Core;
using JWLibrary.Utils.Files;

namespace Service.QueryConst {
    public class QueryJSBase<T>
        where T : class, new() {
        private const string CARRIAGE_RETURN = "\n";
        public static Lazy<T> _instance = new(() => new T());

        public static T Self => _instance.Value;

        protected string ReadQueryJS(string javascriptFile) {
            return javascriptFile.jFileReadLines().@join(CARRIAGE_RETURN);
        }
    }
}
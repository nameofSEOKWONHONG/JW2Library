namespace JWLibrary.Core {
    public interface IENUM_BASE {
        string Value { get; set; }
    }
    
    public class ENUM_BASE<T> : IENUM_BASE 
        where T : IENUM_BASE, new() {
        
        public string Value { get; set; }

        public ENUM_BASE() {
            
        }

        public static T Define(string s) {
            T t = new T();
            t.Value = s;
            return t;
        }

        public override string ToString() {
            return this.Value;
        }
    }
    
    /*
     * test code
     */
    public class ENUM_FLAG_YN : ENUM_BASE<ENUM_FLAG_YN> {
        public static ENUM_FLAG_YN Yes = Define("Y");
        public static ENUM_FLAG_YN No = Define("N");
    }
    
    public class ENUM_USE_YN : ENUM_BASE<ENUM_USE_YN> {
        public static ENUM_USE_YN Y = Define("Y");
        public static ENUM_USE_YN N = Define("N");
    }    
}
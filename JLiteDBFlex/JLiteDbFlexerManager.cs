using System;
using System.Linq;
using JWLibrary.Core;

namespace JLiteDBFlex {
    public class JLiteDbFlexerManager<T> where T : class {
        private static Lazy<JHDictionary<Type, JLiteDbFlexer<T>>> _instance =
            new Lazy<JHDictionary<Type, JLiteDbFlexer<T>>>();
        
        public JLiteDbFlexer<T> Create(string additionalDbFileName = "") {
            var exists = _instance.Value.FirstOrDefault(m => m.Key == typeof(T));
            if (exists.jIsNotNull()) {
                return exists.Value;
            }
            var newInstance = new JLiteDbFlexer<T>(additionalDbFileName);
            if (newInstance.jIsNotNull()) {
                _instance.Value.Add(typeof(T), newInstance);
            }

            return newInstance;
        }
    }

    // public class Test {
    //     public void Run() {
    //         JLiteDbFlexerManager.Instace.Create<TestDto>().LiteCollection.FindAll();
    //         var instance = flexer.Create<TestDto>();
    //         var all = instance.LiteCollection.FindAll();
    //         
    //     }
    // }
    //
    // [JLiteDbTable("./test.db", "test", null, null)]
    // public class TestDto {
    //     
    // }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using JWLibrary.Core;
using NetFabric.Hyperlinq;
using Newtonsoft.Json;
using NiL.JS.Expressions;
using NUnit.Framework;
using Enum = System.Enum;
using Type = System.Type;

namespace JWLibrary.NUnit.Test {
    public class JObjTest {
        [Test]
        public void TypeConvertTest() {
            var src = "10";
            var dest = "".toValue(src);
            Assert.AreEqual(src, dest);

            
            var dest2 = "".toValue<double>(12);
            Assert.AreEqual(dest2, 12);

            // var dest3 = "1".toValue<TYPES>();
            // Assert.AreEqual("1", dest3);
            //
            // var dest4 = TYPES.ONE.toValue<string>();
            // Assert.AreEqual("1", dest4);

            ENUM_USE_YN enumUseYn = ENUM_USE_YN.Y;
            ENUM_USE_YN enumUseYn2 = ENUM_USE_YN.N;

            Assert.AreEqual(enumUseYn, ENUM_USE_YN.Y);
            Assert.AreEqual(enumUseYn2, ENUM_USE_YN.N);
            
            Console.WriteLine(enumUseYn2);
            Console.WriteLine(ENUM_USE_YN.Y);

            var obj = JsonConvert.DeserializeObject<TestObj>(@"{'Name':'test', 'UseYn':'N'}");
            if (obj.isNotNull()) {
                Assert.AreEqual(obj.UseYn, ENUM_USE_YN.N);
            }
        }
    }

    public class TestObj {
        public string Name { get; set; }
        public ENUM_USE_YN UseYn { get; set; }
    }

    public enum TYPES {
        [System.ComponentModel.Description("1")]
        ONE,
        [System.ComponentModel.Description("2")]
        TWO
    }
    
    [JsonConverter(typeof(JsonConverter<ENUM_USE_YN>))]
    public class ENUM_USE_YN : JENUM_BASE<ENUM_USE_YN> {
        public static ENUM_USE_YN Y {get; set;} = define("Y");
        public static ENUM_USE_YN N {get; set;} = define("N");
    }    
}
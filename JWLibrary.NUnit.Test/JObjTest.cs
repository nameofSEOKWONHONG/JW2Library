using System;
using eXtensionSharp;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class JObjTest {
        [Test]
        public void TypeConvertTest() {
            var src = "10";
            var dest = "".xSafe(src);
            Assert.AreEqual(src, dest);


            var dest2 = "".xSafe<double>(12);
            Assert.AreEqual(dest2, 12);

            // var dest3 = "1".jToValue<TYPES>();
            // Assert.AreEqual("1", dest3);
            //
            // var dest4 = TYPES.ONE.jToValue<string>();
            // Assert.AreEqual("1", dest4);

            var enumUseYn = ENUM_USE_YN.Y;
            var enumUseYn2 = ENUM_USE_YN.N;

            Assert.AreEqual(enumUseYn, ENUM_USE_YN.Y);
            Assert.AreEqual(enumUseYn2, ENUM_USE_YN.N);

            Console.WriteLine(enumUseYn2);
            Console.WriteLine(ENUM_USE_YN.Y);

            var obj = JsonConvert.DeserializeObject<TestObj>(@"{'Name':'test', 'UseYn':'N'}");
            if (obj.xIsNotNull()) Assert.AreEqual(obj.UseYn, ENUM_USE_YN.N);
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
    public class ENUM_USE_YN : XEnumBase<ENUM_USE_YN> {
        public static readonly ENUM_USE_YN Y = Define("Y");
        public static readonly ENUM_USE_YN N = Define("N");
    }
}
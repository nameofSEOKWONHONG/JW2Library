using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using JWLibrary.Core;
using NetFabric.Hyperlinq;
using NiL.JS.Expressions;
using NUnit.Framework;
using Enum = System.Enum;
using Type = System.Type;

namespace JWLibrary.NUnit.Test {
    public class JObjTest {
        [Test]
        public void TypeConvertTest() {
            var src = "10";
            var dest = "".jValue(src);
            Assert.AreEqual(src, dest);

            
            var dest2 = "".jValue<double>(12);
            Assert.AreEqual(dest2, 12);

            // var dest3 = "1".jValue<TYPES>();
            // Assert.AreEqual("1", dest3);
            //
            // var dest4 = TYPES.ONE.jValue<string>();
            // Assert.AreEqual("1", dest4);

            ENUM_USE_YN enumUseYn = ENUM_USE_YN.Y;
            ENUM_USE_YN enumUseYn2 = ENUM_USE_YN.N;
             
            Assert.AreEqual(enumUseYn, ENUM_USE_YN.Y);
            Assert.AreEqual(enumUseYn2, ENUM_USE_YN.N);
            
            Console.WriteLine(enumUseYn2);
            Console.WriteLine(ENUM_USE_YN.Y);

        }
    }

    public enum TYPES {
        [System.ComponentModel.Description("1")]
        ONE,
        [System.ComponentModel.Description("2")]
        TWO
    }
}
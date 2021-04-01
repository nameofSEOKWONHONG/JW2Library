using System;
using System.Collections.Generic;
using JWLibrary.Core;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class JDictionaryUtilTest {
        [Test]
        public void JConcatTest() {
            IDictionary<string, string> mapA = new Dictionary<string, string>();
            mapA.Add("A", "A");
            mapA.Add("B", "");
            IDictionary<string, string> mapB = new Dictionary<string, string>();
            mapB.Add("B", "BB");
            mapB.Add("C", "C");

            //if exists same key, throw error 
            //var result = mapA.jConcat(mapB);
            //Assert.AreEqual(result["C"], "C");

            //if exists same key, update second value.
            var result2 = mapA.jConcatUpdate(mapB);
            Assert.AreEqual(result2["B"], "BB");

            Console.WriteLine(result2.jToString());
        }
    }
}
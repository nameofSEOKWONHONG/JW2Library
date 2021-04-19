﻿using System;
using JWLibrary.Core;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class JStringTest {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void JIndexOfTest() {
            var index = "test".IndexOf("");
            Assert.GreaterOrEqual(index, 0);

            index = "test".indexOf("");
            Assert.Less(index, 0);
        }

        [Test]
        public void JIndexOfLastTest() {
            var index = "test".LastIndexOf("");
            Assert.GreaterOrEqual(index, 0);

            index = "test".lastIndexOf("");
            Assert.Less(index, 0);
        }

        [Test]
        public void JSplitTest() {
            var text = "1|2|3|4|5|6|7|8|9|10";
            var result = text.jSplit('|');
            result.jForeach(item => {
                Console.WriteLine(item);
            });
            
            Assert.Pass();
        }

        [Test]
        public void JSubstringTest() {
            var text = "세상이 바뀌어도 그 본질은 바뀌지 않는다.";
            var result = text.jSubstring(4, 1);
            Assert.AreEqual("바", result);
            
        }
    }
}
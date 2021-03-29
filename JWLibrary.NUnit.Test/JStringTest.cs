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

            index = "test".jIndexOf("");
            Assert.Less(index, 0);
        }

        [Test]
        public void JIndexOfLastTest() {
            var index = "test".LastIndexOf("");
            Assert.GreaterOrEqual(index, 0);

            index = "test".jLastIndexOf("");
            Assert.Less(index, 0);
        }
    }
}
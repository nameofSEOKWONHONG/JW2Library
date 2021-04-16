using System.Collections;
using System.Linq;
using JWLibrary.Core;
using NUnit.Framework;
using Org.BouncyCastle.Security;

namespace JWLibrary.NUnit.Test {
    public class EqualsTest {
        [Test]
        public void equals_int_test() {
            Assert.IsTrue(Enumerable.Range(1, 10).isEquals(5));
        }

        [Test]
        public void equals_string_test() {
            Assert.IsTrue((new[] {"A", "B", "C", "D", "E"}).isEquals("B"));
        }

        [Test]
        public void equals_single_test() {
            Assert.IsTrue("A".isEquals("A"));
        }
    }
}
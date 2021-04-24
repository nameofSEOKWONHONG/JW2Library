using System.Linq;
using eXtensionSharp;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class EqualsTest {
        [Test]
        public void equals_int_test() {
            Assert.IsTrue(Enumerable.Range(1, 10).xIsEquals(5));
        }

        [Test]
        public void equals_string_test() {
            Assert.IsTrue(new[] {"A", "B", "C", "D", "E"}.xIsEquals("B"));
        }

        [Test]
        public void equals_single_test() {
            Assert.IsTrue("A".xIsEquals("A"));
        }
    }
}
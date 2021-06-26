using eXtensionSharp;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class JEnumTEst {
        [Test]
        public void jenum_test() {
            TEST_ENUM a = TEST_ENUM.Parse("A");
            Assert.AreEqual(a, TEST_ENUM.A);
        }
    }

    public class TEST_ENUM : XEnumBase<TEST_ENUM> {
        public static readonly TEST_ENUM A = Define("A");
        public static readonly TEST_ENUM B = Define("B");
    }
}
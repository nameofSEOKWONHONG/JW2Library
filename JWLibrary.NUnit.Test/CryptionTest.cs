using System.Security.Cryptography;
using JWLibrary.Utils;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class CryptionTest {
        [Test]
        public void enc_dec_test() {
            var enc = "helloWorld".toEncAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7);
            var dec = enc.toDecAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
            
            Assert.AreEqual("helloWorld", dec);
        }
    }
}
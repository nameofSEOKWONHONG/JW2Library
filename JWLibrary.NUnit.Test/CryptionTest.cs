using System;
using System.Security.Cryptography;
using JWLibrary.Utils;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class CryptionTest {
        [Test]
        public void enc_dec_test() {
            var enc = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=acc;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False".xToEncAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7);
            var dec = enc.xToDecAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7, DeconvertCipherFormat.HEX);
            Console.WriteLine(enc);
            Assert.AreEqual("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=acc;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", dec);

            var enc2 = "Filename=sqlite_test.db".xToEncAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7);
            var enc3 = "Filename=memory".xToEncAes256("asdfasdfasdfasdf", "asdfasdfasdfasdf", CipherMode.CBC, PaddingMode.PKCS7);

            Console.WriteLine(enc2);
            Console.WriteLine(enc3);
        }
    }
}
using JWLibrary.Database;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class SSDBTest {
        private string ip = "192.168.137.245";
        private int port = 8888;
        
        [Test]
        public void ssdb_test1() {
            using (var client = new SSDBClient(ip, port)) {
                client.set("test", "test1");

                var val = string.Empty;
                client.get("test", out val);
                Assert.AreEqual("test1", val);
            }
        }

        [Test]
        public void ssdb_del_test1() {
            using (var client = new SSDBClient(ip, port)) {
                client.del("test");
                var val = string.Empty;
                client.get("test", out val);
                Assert.AreEqual(null, val);
            }
        }
    }
}
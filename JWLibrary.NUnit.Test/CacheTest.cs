using System.Threading;
using JWLibrary.Util.Cache;
using Microsoft.Scripting.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace JWLibrary.NUnit.Test {
    public class CacheTest {
        [Test]
        public void cache_test1() {
            var key = "T1";
            var value = "seokwon";
            var cache = new CacheHandler();
            var result = cache.Add<string>(key, value);
            Assert.IsTrue(result);
            var resultValue = cache.Get<string>(key);
            Assert.AreEqual(value, resultValue.ValueObject);
            var delete = cache.Delete(key);
            Assert.IsTrue(delete);

            Assert.GreaterOrEqual(cache.Count(), 2);

        }

        [Test]
        public void cache_test2() {
            var key = "T1";
            var value = "seokwon";
            var result = CacheResolver.Instance.Resolve<TestNameResolver, string>();
            Assert.AreEqual(value, result);
            Thread.Sleep(6000);
            result = CacheResolver.Instance.Resolve<TestNameResolver, string>();
            Assert.AreEqual(value, result);
            var cache = new CacheHandler();
            var deletedKey = cache.Get<string>(key);
            Assert.IsNull(deletedKey);
        }
    }

    public class TestNameResolver : CacheResolverBase<string> {
        public override string InitKey() {
            return "T1";
        }

        public override string GetValue() {
            return "seokwon";
        }

        public override int ResetInterval() {
            return 5;
        }
    }
}
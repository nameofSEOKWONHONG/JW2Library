using System;
using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.EF;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;

namespace JWLibrary.NUnit.Test {
    public class RedisClientTest {
        [Test]
        public void redis_value_type_test() {
            RedisClientHandler handler = new RedisClientHandler(NoSqlConnectionProvider.Instance.REDIS);
            handler.Execute(o => {
                o.Set<string>("test", "test");
                var str = o.Get<string>("test");
                Assert.AreEqual("test", str.xSafe());
            });
            
            handler.Execute(o => {
                o.Set<int>("test1", 1);
                var num = o.Get<int>("test1");
                Assert.AreEqual(1, num);
                
            });
        }

        [Test]
        public void redis_class_type_test() {
            var handler = new RedisClientHandler(NoSqlConnectionProvider.Instance.REDIS);
            handler.Execute(db => {
                var blog = new Blog();
                blog.ID = 1;
                blog.BLOG_NAME = "test";
                blog.BLOG_AUTHOR = "test";
                blog.WRITE_DT = DateTime.Now;

                var seted = db.Set<Blog>("blog", blog);
                Assert.IsTrue(seted);

                var exists = db.Get<Blog>("blog");
                Assert.NotNull(exists);
                Assert.AreEqual(1, exists.ID);

                var removed = db.KeyDelete("blog");
                Assert.IsTrue(removed);

                exists = db.Get<Blog>("blog");
                Assert.IsTrue(exists.xIsNull());
            });
        }
    }

    
}
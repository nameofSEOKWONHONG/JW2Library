﻿using System;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.EF;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class MongodbTest {
        [Test]
        public void mongodb_test() {
            var handler = new MongoClientHandler(NoSqlConnectionProvider.Instance.MONGODB, "test_db");
            handler.Execute<Blog>(collection => {
                var blog = new Blog() {
                    ID = 1,
                    BLOG_NAME = "test",
                    BLOG_AUTHOR = "test",
                    WRITE_DT = DateTime.Now
                };
                var document = BsonDocument.Parse(blog.xToJson());
                collection.InsertOne(document);
            });
            
            handler.Execute<Blog>(collection => {
                var filter = Builders<BsonDocument>.Filter.Eq("BLOG_NAME", "test");
                var exists = collection.Find(filter).FirstOrDefault();
                Assert.IsTrue(exists.xIsNotNull());

                var blog = exists.xToEntity<Blog>();
                Assert.AreEqual(blog.BLOG_NAME, "test");
            });
            
            handler.Execute<Blog>(collection => {
                var filter = Builders<BsonDocument>.Filter.Eq("BLOG_NAME", "test");
                collection.DeleteMany(filter);

                var exists = collection.Find(filter).FirstOrDefault();
                Assert.IsTrue(exists.xIsNull());
            });
        }
    }
    
}
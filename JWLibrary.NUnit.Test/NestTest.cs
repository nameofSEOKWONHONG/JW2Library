using System;
using eXtensionSharp;
using Nest;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class NestTest {
        [Test]
        public void nest_test() {
            var node = new Uri("http://192.168.137.245:9200/");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            
            var tweet = new Tweet
            {
                Id = 1,
                User = "kimchy",
                PostDate = new DateTime(2009, 11, 15),
                Message = "Trying out NEST, so far so good?"
            };

            var response = client.Index(tweet, idx => idx.Index("mytweetindex")); //or specify index via settings.DefaultIndex("mytweetindex");
            if (response.xIsNull()) {
                
            }
        }
    }

    public class Tweet {
        public int Id { get; set; }
        public string User { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
    }
}
using JLiteDBFlex;
using JWLibrary.Core;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class LiteDbFlexerTest {
        [Test]
        public void insert_test() {
            var flexer = new LiteDbFlexer<LiteDbFlexerTestDto>();
            var result = flexer.LiteCollection.Insert(new LiteDbFlexerTestDto() {
                Name = "test",
                Age = 18
            });
            
            Assert.Greater(result.AsInt32, 0);
        }

        [Test]
        public void get_test() {
            var flexer = new LiteDbFlexer<LiteDbFlexerTestDto>();
            var exists =flexer.LiteCollection.FindOne(m => m.Name == "test");
            Assert.NotNull(exists);
        }

        [Test]
        public void get_delete_test() {
            var flexer = new LiteDbFlexer<LiteDbFlexerTestDto>();
            var exists = flexer.LiteCollection.FindOne(m => m.Name == "test");
            if (exists.isNotNull()) {
                flexer.LiteCollection.Delete(exists.Id);
            }

            exists = flexer.LiteCollection.FindOne(m => m.Name == "test");
            
            Assert.IsNull(exists);
        }

        [Test]
        public void get_update_test() {
            var flexer = new LiteDbFlexer<LiteDbFlexerTestDto>();
            var exists = flexer.LiteCollection.FindOne(m => m.Name == "test");
            if (exists.isNotNull()) {
                exists.Age = 22;
                var result= flexer.LiteCollection.Update(exists);
                Assert.IsTrue(result);
            }
            
            exists = flexer.LiteCollection.FindOne(m => m.Name == "test");
            Assert.AreEqual(22, exists.Age);
        }
    }

    [LiteDbTable("litedbflexer.db", "flex", new [] {"Name"})]
    public class LiteDbFlexerTestDto {
        //auto-increment
        //id가 명시적으로 없을 경우 _oid를 생성함. 해당 hash값이 고유값이 됨. Key와는 다른 의미임.
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
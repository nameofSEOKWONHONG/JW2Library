using JWService.Data.Models;
using LiteDB;
using LiteDbFlex;
using NUnit.Framework;

namespace Service.Test {
    public class MemberTest {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void AddAccount() {
            var result = LiteDbFlexerManager<Account>.Instance.Value.Create()
                .BeginTrans()
                .Insert(new Account() {
                    Id = 0,
                    UserId = "test",
                    Passwd = "test"
                })
                .Commit()
                .GetResult<BsonValue>();

            Assert.Greater((int)result, 0);
        }

        [Test]
        public void GetAccount() {
            var account = LiteDbFlexerManager<Account>.Instance.Value.Create()
                .Get(m => m.UserId == "test" && m.Passwd == "test")
                .GetResult<Account>();

            Assert.NotNull(account);
        }
    }
}
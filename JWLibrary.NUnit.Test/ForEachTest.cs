using System;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using JLiteDBFlex;
using NUnit.Framework;

namespace JWLibrary.NUnit.Test {
    public class ForEachTest {
        [Test]
        public void foreach_test() {
            var num = 0;
            Enumerable.Range(1, 10).xForEach(i => {
                num += i;
                Console.WriteLine(num);
            });

            Assert.Greater(num, 0);
        }

        [Test]
        public void foreach_async_test() {
            var num = 0;
            Enumerable.Range(1, 10).jForeachAsync(async i => {
                await Task.Factory.StartNew(() => {
                    num += i;
                    Console.WriteLine(num);
                });
            });

            Assert.Greater(num, 0);
        }

        [Test]
        public void foreach_async_test2() {
            var manager = LiteDbFlexerManager.Instance.Create<UserDto>();
            manager.LiteDatabase.DropCollection(manager.TableName);

            var users = new XList<UserDto>();
            users.Add(new UserDto {Name = "a", Age = 1});
            users.Add(new UserDto {Name = "b", Age = 2});
            users.Add(new UserDto {Name = "c", Age = 3});

            users.jForeachAsync(item => {
                var inserted = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection.Insert(item);
                Assert.NotNull(inserted.AsObjectId);
                return Task.CompletedTask;
            });
        }

        [Test]
        public void foreach_async_result_test() {
            var users = LiteDbFlexerManager.Instance.Create<UserDto>().LiteCollection
                .FindAll()
                .xWhere(m => m.Name != null);

            users.jForeachAsync(item => {
                Console.WriteLine(item.xFromObjectToJson());
                Assert.NotNull(item);
                return Task.CompletedTask;
            });
        }
    }
}
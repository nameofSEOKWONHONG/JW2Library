using JWLibrary.ServiceExecutor;
using NUnit.Framework;
using Service;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JWLibrary.NUnit.Test {

    public class MigrationTest {
        private IMigrationDatabaseService _initDatabaseService;

        [SetUp]
        public void Setup() {
            _initDatabaseService = new MigrationDatabaseService();
        }

        [Test]
        public void UserMigrationTest() {
            var result = false;
            using (var executor = new ServiceExecutorManager<IMigrationDatabaseService>(this._initDatabaseService)) {
                executor.SetRequest(o => o.Request = true)
                    .OnExecuted(o => {
                        result = o.Result;
                    });
            }
            Console.WriteLine(result);

            Assert.True(result);
        }

        [Test]
        public async Task UserMigrationTestAsync() {
            var result = false;
            using (var executor = new ServiceExecutorManager<IMigrationDatabaseService>(this._initDatabaseService)) {
                await executor.SetRequest(o => o.Request = true)
                    .OnExecutedAsync(o => {
                        result = o.Result;
                        return Task.CompletedTask;
                    });
            }

            Assert.True(result);
        }
    }
}
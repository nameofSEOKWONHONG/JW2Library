// using System;
// using System.Threading.Tasks;
// using JWLibrary.ServiceExecutor;
// using NUnit.Framework;
// using Service;
//
// namespace JWLibrary.NUnit.Test {
//     public class MigrationTest {
//         private IMigrationDatabaseService _initDatabaseService;
//
//         [SetUp]
//         public void Setup() {
//             _initDatabaseService = new MigrationDatabaseService();
//         }
//
//         [Test]
//         public void UserMigrationTest() {
//             var result = false;
//             using (var executor = new ServiceExecutorManager<IMigrationDatabaseService>(_initDatabaseService)) {
//                 executor.SetRequest(o => o.Request = true)
//                     .OnExecuted(o => {
//                         result = o.Result;
//                         return true;
//                     });
//             }
//
//             Console.WriteLine(result);
//
//             Assert.True(result);
//         }
//
//         [Test]
//         public async Task UserMigrationTestAsync() {
//             var result = false;
//             using (var executor = new ServiceExecutorManager<IMigrationDatabaseService>(_initDatabaseService)) {
//                 await executor.SetRequest(o => o.Request = true)
//                     .OnExecutedAsync(async o => {
//                         result = o.Result;
//                         return true;
//                     });
//             }
//
//             Assert.True(result);
//         }
//     }
// }
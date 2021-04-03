// using System;
// using System.IO;
// using System.Linq;
// using FluentValidation;
// using JWLibrary.Core;
// using JWLibrary.ServiceExecutor;
//
// namespace JCoreSvcTest {
//     internal class Test1 {
//         public string GUID = string.Empty;
//
//         public Test1() {
//             GUID = Guid.NewGuid().ToString();
//             Console.WriteLine($"ctor test {GUID}");
//         }
//
//         public void Run() {
//             Console.WriteLine("run...");
//         }
//     }
//
//     internal class Program {
//         private static readonly Lazy<JLKList<Test1>> _instance =
//             new(() => new JLKList<Test1>());
//
//         private static Test1 Self(string name) {
//             return _instance.Value.Where(m => m.GUID == name).FirstOrDefault();
//         }
//
//         /// <summary>
//         ///     JWLibrary.Test (DragonFruit-Test)
//         /// </summary>
//         /// <param name="intOption">An option whose argument will bind to an int</param>
//         /// <param name="boolOption">An option whose argument will bind to a bool</param>
//         /// <param name="fileOption">An option whose argument will bind to a FileInfo</param>
//         private static void Main(int intOption = 42, bool boolOption = false, FileInfo fileOption = null) {
//             if (fileOption != null && !fileOption.Exists) fileOption.Create();
//             Console.WriteLine($"The value of intOption is: {intOption}");
//             Console.WriteLine($"The value of boolOption is: {boolOption}");
//             Console.WriteLine($"The value of fileOption is: {fileOption?.FullName ?? "null"}");
//
//             // _instance.Value.Add(new Test1());
//             // _instance.Value.Add(new Test1());
//             // _instance.Value.Add(new Test1());
//             // _instance.Value.Add(new Test1());
//             // _instance.Value.Add(new Test1());
//             // var guid = Console.ReadLine();
//             //
//             // var instance = Self(guid);
//             // instance.Run();
//
//             // ITestService service = new TestService();
//             //
//             // var result = string.Empty;
//             // using (var executor = new ServiceExecutorManager<ITestService>(service)) {
//             //     executor.SetRequest(o => o.Request = "Service Executor ")
//             //         .AddFilter(o => o.Request.Length > 0)
//             //         .AddFilter(o => o.Request.jIsNotNull())
//             //         .OnExecuted(o => {
//             //             result = o.Result;
//             //         });
//             // }
//
// //             var pysrc = @"
// // a = 10
// // c = a + b
// // ";
// //             pysrc.Execute_Python_Script_Async(
// //                 (s) =>
// //                 {
// //                     s.SetVariable("b", 20);
// //                 },
// //                 (s) =>
// //                 {
// //                     Console.WriteLine(s.GetVariable("c"));
// //                 });
//
// //             var sql = @"
// // name = 'seokwon'
// // age = 18
// // sql = 'SELECT * FROM CUSTOMER WHERE 1=1'
// //
// // if name != '':
// //     sql += ''' AND NAME = '{}' '''.format(name)
// //
// // if age != '':
// //     sql += ''' AND AGE = {} '''.format(age)    
// //
// // if v != '':
// //     sql += ''' AND VALUE = '{}' '''.format(v)
// // ";
// //             sql.Execute_Python_Script_Async((pre) =>
// //             {
// //                 pre.SetVariable("v", "str");
// //             }, (end) =>
// //             {
// //                 Console.WriteLine(end.GetVariable("sql"));
// //             });
//
//             // var sqlfile = "./query.py";
//             // var sql = string.Empty;
//             // sqlfile.Execute_Python_File_Async((pre) =>
//             // {
//             //     pre.SetVariable("name", "seokwon");
//             //     pre.SetVariable("age", 10);
//             //     pre.SetVariable("v", "str");
//             // }, (end) =>
//             // {
//             //     sql = end.GetVariable("resultSql");
//             // });
//             //
//             // Console.WriteLine(sql);
// //             var javascript = @"
// // var result = x * 2;
// // ";
// //             javascript.ExecuteJavascriptSource((c) =>
// //             {
// //                 c.DefineVariable("x").Assign(123);
// //             }, (c) =>
// //             {
// //                 var result = c.GetVariable("result");
// //                 Console.WriteLine(result);
// //                 Console.WriteLine(result.Is<int>());
// //                 Console.WriteLine(result.ValueType);
// //             });
// //             var sql = @"
// // var sql = '';
// // sql += 'SELECT * FROM CUSTOMER WHERE 1=1';
// // if(name != '')
// //     sql += ` AND NAME = '${name}' `;
// // if(age > 0)
// //     sql += ` AND AGE > ${age} `;
// // if(v != '')
// //     sql += ` AND VALUE = '${v}' `;
// // ";
// //             sql.Execute((c) =>
// //             {
// //                 c.DefineVariable("name").Assign("seokwon");
// //                 c.DefineVariable("age").Assign(18);
// //                 c.DefineVariable("v").Assign("str");
// //             }, (c) =>
// //             {
// //                 var result = c.GetVariable("sql");
// //                 Console.WriteLine(result);
// //             } );
// //
// //             var sqlfile = "./query.js";
// //             sqlfile.ExecuteFile((c) =>
// //             {
// //                 c.DefineVariable("name").Assign("seokwon");
// //                 c.DefineVariable("age").Assign(18);
// //                 c.DefineVariable("v").Assign("str");
// //             }, (c) =>
// //             {
// //                 var result = c.GetVariable("sql");
// //                 Console.WriteLine(result);
// //             } );
//         }
//     }
//
//     /// <summary>
//     /// </summary>
//     public interface ITestService : IServiceExecutor<string, string> {
//     }
//
//     /// <summary>
//     /// </summary>
//     public class TestService : ServiceExecutor<TestService, string, string>, ITestService {
//         /// <summary>
//         /// </summary>
//         public TestService() {
//             base.SetValidator(new TestServiceValidator());
//         }
//
//         /// <summary>
//         /// </summary>
//         public override void Execute() {
//             Result = $"{Request}_Hello_World";
//         }
//
//         /// <summary>
//         /// </summary>
//         private class TestServiceValidator : AbstractValidator<TestService> {
//             /// <summary>
//             /// </summary>
//             public TestServiceValidator() {
//                 RuleFor(o => o.Request).NotNull();
//             }
//         }
//     }
// }
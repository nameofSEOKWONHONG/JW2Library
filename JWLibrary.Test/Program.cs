using System;
using System.IO;
using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Util;
using NiL.JS.Extensions;

namespace JCoreSvcTest {
    class Program {
        static void Main(string[] args) {
            // ITestService service = new TestService();
            //
            // var result = string.Empty;
            // using (var executor = new ServiceExecutorManager<ITestService>(service)) {
            //     executor.SetRequest(o => o.Request = "Service Executor ")
            //         .AddFilter(o => o.Request.Length > 0)
            //         .AddFilter(o => o.Request.jIsNotNull())
            //         .OnExecuted(o => {
            //             result = o.Result;
            //         });
            // }

//             var pysrc = @"
// a = 10
// c = a + b
// ";
//             pysrc.Execute_Python_Script_Async(
//                 (s) =>
//                 {
//                     s.SetVariable("b", 20);
//                 },
//                 (s) =>
//                 {
//                     Console.WriteLine(s.GetVariable("c"));
//                 });

//             var sql = @"
// name = 'seokwon'
// age = 18
// sql = 'SELECT * FROM CUSTOMER WHERE 1=1'
//
// if name != '':
//     sql += ''' AND NAME = '{}' '''.format(name)
//
// if age != '':
//     sql += ''' AND AGE = {} '''.format(age)    
//
// if v != '':
//     sql += ''' AND VALUE = '{}' '''.format(v)
// ";
//             sql.Execute_Python_Script_Async((pre) =>
//             {
//                 pre.SetVariable("v", "str");
//             }, (end) =>
//             {
//                 Console.WriteLine(end.GetVariable("sql"));
//             });

            // var sqlfile = "./query.py";
            // var sql = string.Empty;
            // sqlfile.Execute_Python_File_Async((pre) =>
            // {
            //     pre.SetVariable("name", "seokwon");
            //     pre.SetVariable("age", 10);
            //     pre.SetVariable("v", "str");
            // }, (end) =>
            // {
            //     sql = end.GetVariable("resultSql");
            // });
            //
            // Console.WriteLine(sql);
//             var javascript = @"
// var result = x * 2;
// ";
//             javascript.ExecuteJavascriptSource((c) =>
//             {
//                 c.DefineVariable("x").Assign(123);
//             }, (c) =>
//             {
//                 var result = c.GetVariable("result");
//                 Console.WriteLine(result);
//                 Console.WriteLine(result.Is<int>());
//                 Console.WriteLine(result.ValueType);
//             });
            var sql = @"
var sql = '';
sql += 'SELECT * FROM CUSTOMER WHERE 1=1';
if(name != '')
    sql += ` AND NAME = '${name}' `;
if(age > 0)
    sql += ` AND AGE > ${age} `;
if(v != '')
    sql += ` AND VALUE = '${v}' `;
";
            sql.Execute((c) =>
            {
                c.DefineVariable("name").Assign("seokwon");
                c.DefineVariable("age").Assign(18);
                c.DefineVariable("v").Assign("str");
            }, (c) =>
            {
                var result = c.GetVariable("sql");
                Console.WriteLine(result);
            } );

            var sqlfile = "./query.js";
            sqlfile.ExecuteFile((c) =>
            {
                c.DefineVariable("name").Assign("seokwon");
                c.DefineVariable("age").Assign(18);
                c.DefineVariable("v").Assign("str");
            }, (c) =>
            {
                var result = c.GetVariable("sql");
                Console.WriteLine(result);
            } );
        }
    }

    public interface ITestService : IServiceExecutor<string, string> { }

    public class TestService : ServiceExecutor<TestService, string, string>, ITestService {
        public TestService() {
            base.SetValidator(new TestServiceValidator());
        }

        public override void Execute() {
            this.Result = $"{this.Request}_Hello_World";
        }

        private class TestServiceValidator : AbstractValidator<TestService> {
            public TestServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}

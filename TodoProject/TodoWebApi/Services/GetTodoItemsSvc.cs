using System.Collections;
using System.Collections.Generic;
using Dapper;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    public class GetTodoItemsSvc : ServiceExecutor<GetTodoItemsSvc, string, IEnumerable<TODO>>,
        IGetTodoItemsSvc{
        public GetTodoItemsSvc() {
            
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    this.Owner.Result = db.GetList<TODO>();
                });
        }
    }
}
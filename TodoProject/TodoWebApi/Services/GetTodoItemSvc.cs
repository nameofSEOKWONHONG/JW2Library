//no use System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Dapper;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using TodoWebApi.Entities;

namespace TodoWebApi.Services {
    public class GetTodoItemSvc : ServiceExecutor<GetTodoItemSvc, int, TODO>, IGetTodoItemSvc {
        public GetTodoItemSvc() {
            
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .DbExecutor(db => {
                    this.Owner.Result = db.Get<TODO>(this.Owner.Request);
                });
        }
    }
}
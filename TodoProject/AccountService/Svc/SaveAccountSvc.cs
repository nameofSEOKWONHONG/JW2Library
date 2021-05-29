using System.Collections.Generic;
using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using SqlKata.Execution;
using TodoService.Data;
using Microsoft.Data.SqlClient;

namespace AccountService.Svc {

    public class SaveAccountSvc : ServiceExecutor<SaveAccountSvc, USER, int>, 
        ISaveAccountSvc {
        private USER _exists;
        private readonly IGetAccountSvc _getAccountSvc;
        public SaveAccountSvc(IGetAccountSvc getAccountSvc) {
            _getAccountSvc = getAccountSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(_getAccountSvc);
            return executor.SetRequest(o => o.Request = Request.USER_ID)
                .OnExecuted(o => {
                    _exists = o.Result;
                    return true;
                });
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .AddTran()
                .DbExecuteKata((db, q) => {
                    if (_exists.xIsNotNull()) {
                        this.Result = q.Query("USER").Where("ID", this.Request.USER_ID)
                            .Update(this.Request.xToDictionary());
                    }
                    this.Result = q.Query("TODO").InsertGetId<int>(this.Request.xToDictionary());
                });
        }
    }
}
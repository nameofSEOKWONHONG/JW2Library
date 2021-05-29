using eXtensionSharp;
using JWLibrary.Database;
using JWLibrary.ServiceExecutor;
using Microsoft.Data.SqlClient;
using SqlKata.Execution;
using TodoService.Data;

namespace AccountService.Svc {

    public class DeleteAccountSvc : ServiceExecutor<DeleteAccountSvc, int, bool>, 
        IDeleteAccountSvc {
        private USER _exists;
        private readonly IGetAccountSvc _getAccountSvc;
        public DeleteAccountSvc(IGetAccountSvc getAccountSvc) {
            _getAccountSvc = getAccountSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(_getAccountSvc);
            return executor.SetRequest(o => o.Request = this.Request)
                .OnExecuted(o => {
                    _exists = o.Result;
                    return _exists.xIsNotNull();
                });
        }

        public override void Execute() {
            JDatabaseResolver.Resolve<SqlConnection>()
                .AddTran()
                .DbExecuteKata((db, q) => {
                    this.Result = q.Query("USER").Where("ID", this.Request).Delete() > 0;
                });
        }
    }
}
using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWLibrary.Web;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    public class MemberController : JControllerBase<MemberController> {
        private readonly ISaveAccountSvc _saveAccountSvc;
        private readonly IGetAccountsSvc _getAccountsSvc;
        private readonly IGetAccountSvc _getAccountSvc;
        private readonly IDeleteAccountSvc _deleteAccountSvc;
        private readonly IGetAccountByIdSvc _getAccountByIdSvc;
        public MemberController(ILogger<MemberController> logger,
            ISaveAccountSvc saveAccountSvc,
            IGetAccountsSvc getAccountsSvc,
            IGetAccountSvc getAccountSvc,
            IDeleteAccountSvc deleteAccountSvc,
            IGetAccountByIdSvc getAccountByIdSvc) : base(logger) {
            this._saveAccountSvc = saveAccountSvc;
            this._getAccountsSvc = getAccountsSvc;
            this._getAccountSvc = getAccountSvc;
            this._deleteAccountSvc = deleteAccountSvc;
            this._getAccountByIdSvc = getAccountByIdSvc;
        }

        [HttpPost]
        public async Task<bool> SaveMember([FromBody] Account account) => await this.ExecuteServiceAsync<ISaveAccountSvc, Account, bool>(this._saveAccountSvc, 
            account);

        [Authorize]
        [HttpGet]
        public async Task<PagingResultDto<IEnumerable<Account>>> GetMembers([FromQuery] PagingRequestDto<Account> pagingRequestDto)
            => await this.ExecuteServiceAsync<IGetAccountsSvc, PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>>(this._getAccountsSvc, 
                pagingRequestDto);

        [Authorize]
        [HttpGet]
        public async Task<Account> GetMember(string userId, string passwd) =>
            await this.ExecuteServiceAsync<IGetAccountSvc, Account, Account>(this._getAccountSvc,
                new Account() {UserId = userId, Passwd = passwd},
                (o) => o.Request.jIsNotNull());

        [Authorize]
        [HttpDelete]
        public async Task<bool> DeleteMember(int id) =>
            await this.ExecuteServiceAsync<IDeleteAccountSvc, RequestDto<int>, bool>(this._deleteAccountSvc, 
                new RequestDto<int>(){Data = id},
                (o) => o.Request.Data > 0);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using APIServer.Config;
using eXtensionSharp;
using JWLibrary;
using JWLibrary.Web;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Data;

namespace APIServer.Controllers {
    public class MemberController : JController<MemberController> {
        private readonly IDeleteAccountSvc _deleteAccountSvc;
        private readonly IGetAccountByIdSvc _getAccountByIdSvc;
        private readonly IGetAccountsSvc _getAccountsSvc;
        private readonly IGetAccountSvc _getAccountSvc;
        private readonly ISaveAccountSvc _saveAccountSvc;

        public MemberController(ILogger<MemberController> logger,
            ISaveAccountSvc saveAccountSvc,
            IGetAccountsSvc getAccountsSvc,
            IGetAccountSvc getAccountSvc,
            IDeleteAccountSvc deleteAccountSvc,
            IGetAccountByIdSvc getAccountByIdSvc) : base(logger) {
            _saveAccountSvc = saveAccountSvc;
            _getAccountsSvc = getAccountsSvc;
            _getAccountSvc = getAccountSvc;
            _deleteAccountSvc = deleteAccountSvc;
            _getAccountByIdSvc = getAccountByIdSvc;
        }

        [HttpPost]
        public async Task<bool> SaveMember([FromBody] Account account) {
            return await CreateServiceAsync<ISaveAccountSvc, Account, bool>(_saveAccountSvc,
                account);
        }

        [Authorize]
        [HttpGet]
        public async Task<PagingResultDto<IEnumerable<Account>>> GetMembers(
            [FromQuery] PagingRequestDto<Account> pagingRequestDto) {
            return await
                CreateServiceAsync<IGetAccountsSvc, PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>>(
                    _getAccountsSvc,
                    pagingRequestDto);
        }

        [Authorize]
        [HttpGet]
        public async Task<Account> GetMember(string userId, string passwd) {
            return await CreateServiceAsync<IGetAccountSvc, Account, Account>(_getAccountSvc,
                new Account {UserId = userId, Passwd = passwd},
                o => o.Request.xIsNotNull());
        }

        [Authorize]
        [HttpDelete]
        public async Task<bool> DeleteMember(int id) {
            return await CreateServiceAsync<IDeleteAccountSvc, RequestDto<int>, bool>(_deleteAccountSvc,
                new RequestDto<int> {Data = id},
                o => o.Request.Data > 0);
        }
    }
}
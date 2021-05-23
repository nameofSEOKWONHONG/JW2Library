using System;
using System.Collections;
using System.Collections.Generic;
using JWLibrary.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Config;

namespace APIServer.Controllers {
    [ApiVersion("1")]
    public class ConfigStorageController : JVersionControllerBase<ConfigStorageController> {
        private IGetConfigSvc _getConfigSvc;
        private IGetListConfigSvc _getListConfigSvc;
        private ISaveConfigSvc _saveConfigSvc;
        private IBulkSaveConfigSvc _bulkSaveConfigSvc;
        public ConfigStorageController(ILogger<ConfigStorageController> logger,
            IGetConfigSvc getConfigSvc,
            IGetListConfigSvc getListConfigSvc,
            ISaveConfigSvc saveConfigSvc,
            IBulkSaveConfigSvc bulkSaveConfigSvc) : base(logger) {
            this._getConfigSvc = getConfigSvc;
            this._getListConfigSvc = getListConfigSvc;
            this._saveConfigSvc = saveConfigSvc;
            this._bulkSaveConfigSvc = bulkSaveConfigSvc;
        }

        [HttpGet]
        public GetConfigResult Get(string key) =>
            this.CreateService<IGetConfigSvc, GetConfigRequest, GetConfigResult>(this._getConfigSvc, new GetConfigRequest() {Key = key});

        [HttpGet]
        public IEnumerable<GetConfigResult> GetAll(IEnumerable<GetConfigRequest> keys) =>
            this.CreateService<IGetListConfigSvc, IEnumerable<GetConfigRequest>, IEnumerable<GetConfigResult>>(this._getListConfigSvc,
                keys);

        [HttpPost]
        public SaveConfigResult Save([FromBody] SaveConfigRequest request) {
            return this.CreateService<ISaveConfigSvc, SaveConfigRequest, SaveConfigResult>(this._saveConfigSvc,
                request);
        }

        [HttpPost]
        public IEnumerable<SaveConfigResult> BulkSave([FromBody] IEnumerable<SaveConfigRequest> requests) {
            return this.CreateService<IBulkSaveConfigSvc, IEnumerable<SaveConfigRequest>, IEnumerable<SaveConfigResult>>(
                this._bulkSaveConfigSvc, requests);
        }





    }
}
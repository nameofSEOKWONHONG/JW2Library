using System;
using System.Collections;
using System.Collections.Generic;
using FluentValidation;
using JWLibrary.ServiceExecutor;
using Newtonsoft.Json.Linq;

namespace Service.Config {
    public interface IGetConfigSvc : IServiceExecutor<GetConfigRequest, GetConfigResult> {
        
    }

    public interface ISaveConfigSvc : IServiceExecutor<SaveConfigRequest, SaveConfigResult> {
        
    }

    public interface IGetListConfigSvc : IServiceExecutor<IEnumerable<GetConfigRequest>, IEnumerable<GetConfigResult>> {
        
    }

    public interface IBulkSaveConfigSvc : IServiceExecutor<IEnumerable<SaveConfigRequest>, IEnumerable<SaveConfigResult>> {
        
    }

    public class GetConfigRequest {
        public string Key { get; set; }
    }

    public class GetConfigResult {
        public string Key { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string WriteDt { get; set; }
    }
    
    public class SaveConfigRequest {
        public string Content { get; set; }
        public string UserId { get; set; }
        public string WriteDt { get; set; }
        
        public class Validator : AbstractValidator<SaveConfigRequest> {
            public Validator() {
                RuleFor(m => m.Content).NotNull().NotEmpty();
                RuleFor(m => m.UserId).NotNull().NotEmpty();
                RuleFor(m => m.WriteDt).NotNull().NotEmpty();
            }
        }
    }

    public class SaveConfigResult {
        public bool IsSuccess { get; set; }
        public string Key { get; set; }
    }
}
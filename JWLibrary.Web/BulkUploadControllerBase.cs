using JWLibrary.Core;
using JWLibrary.Core.Data.BulkUpload;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Web {
    public class BulkUploadControllerBase : JControllerBase<BulkUploadControllerBase> {
        public BulkUploadControllerBase(ILogger<BulkUploadControllerBase> logger) : base(logger) {
        }

        public virtual bool Upload<T>(BulkUploadDto<T>[] items)
            where T : class {
            IBulkUploadValidator<T> validator = new BulkUploadValidator<T>();
            items.jForeach(item => {
                validator.Validate(item);
                return true;
            });
            return true;
        }
    }

    public class BulkUploadValidator<T> : IBulkUploadValidator<T>
        where T : class {
        public void Validate(BulkUploadDto<T> item) {
            if (item.Data.jIsNull()) {
                item.IsValid = false;
                item.ErrorMsg = "Data is null";
            }
        }
    }

    public interface IBulkUploadValidator<T>
        where T : class {
        void Validate(BulkUploadDto<T> item);
    }
}
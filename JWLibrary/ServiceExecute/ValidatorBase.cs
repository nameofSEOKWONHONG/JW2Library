using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public interface IValidatorBase<T> { }
    public class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase<T>
        where T : class {

        public ValidatorBase() {

        }
    }
}

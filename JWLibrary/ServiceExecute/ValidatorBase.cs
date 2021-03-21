using FluentValidation;

namespace JWLibrary.ServiceExecutor {
    public interface IValidatorBase<T> {
    }

    public class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase<T>
        where T : class {
    }
}
using SneakersShop.ViewModels;
using FluentValidation.Results;

namespace SneakersShop.Validators.Interfaces
{
    public interface IAuthViewModelValidator
    {
        ValidationResult Validate(AuthViewModel vm);
    }
}

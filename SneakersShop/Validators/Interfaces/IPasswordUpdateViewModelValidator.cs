using FluentValidation.Results;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Interfaces
{
    public interface IPasswordUpdateViewModelValidator
    {
        ValidationResult Validate(PasswordUpdateViewModel vm);
    }
}

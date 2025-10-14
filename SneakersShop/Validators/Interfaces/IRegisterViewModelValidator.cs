using FluentValidation.Results;
using SneakersShop.ViewModels;


namespace SneakersShop.Validators.Interfaces
{
    public interface IRegisterViewModelValidator
    {
        ValidationResult Validate(RegisterViewModel vm);
    }
}

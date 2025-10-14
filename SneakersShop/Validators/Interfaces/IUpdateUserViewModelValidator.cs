using FluentValidation.Results;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Interfaces
{
    public interface IUpdateUserViewModelValidator
    {
        ValidationResult Validate(ProfileUpdateViewModel vm);
    }
}

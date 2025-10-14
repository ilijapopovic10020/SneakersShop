using FluentValidation.Results;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Interfaces
{
    public interface ICheckoutViewModelValidator
    {
        ValidationResult Validate(CheckoutViewModel vm);
    }
}

using FluentValidation;
using FluentValidation.Results;
using SneakersShop.Models;
using SneakersShop.Validators.Interfaces;
using SneakersShop.ViewModels;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SneakersShop.Validators.Implementations
{
    public class CheckoutViewModelValidator : ICheckoutViewModelValidator
    {
        private readonly AbstractValidator<CheckoutViewModel> _validator;

        public CheckoutViewModelValidator()
        {
            _validator = new InlineValidator<CheckoutViewModel>();

            _validator.When(x => x.SelectedPaymentType == PaymentType.Card, () =>
            {
                _validator.RuleFor(x => x.CardNumber)
                    .NotNull().WithMessage("Broj kartice je obavezno polje.")
                    .Must(x => !string.IsNullOrWhiteSpace(x?.Value))
                        .WithMessage("Broj kartice je obavezno polje.")
                    .Must(x => Regex.IsMatch(x?.Value ?? "", @"^\d{4} \d{4} \d{4} \d{4}$"))
                        .WithMessage("Broj kartice mora biti u formatu: xxxx xxxx xxxx xxxx.")
                    .Must(x => x != null && (x.Value.StartsWith("4") || x.Value.StartsWith("5") || x.Value.StartsWith("2")))
                        .WithMessage("Podržane su samo Visa i MasterCard kartice.");

                _validator.RuleFor(x => x.CardHolder)
                    .NotNull().WithMessage("Vlasnik kartice je obavezno polje.")
                    .Must(x => !string.IsNullOrWhiteSpace(x?.Value))
                        .WithMessage("Vlasnik kartice je obavezno polje.")
                    .Must(x => Regex.IsMatch(x?.Value ?? "", @"^[A-ZČĆŽŠĐa-zčćžšđ]+\s[A-ZČĆŽŠĐa-zčćžšđ]+$"))
                        .WithMessage("Vlasnik kartice nije u ispravnom formatu.");

                _validator.RuleFor(x => x.Cvv)
                    .NotNull().WithMessage("Cvv je obavezno polje.")
                    .Must(x => !string.IsNullOrWhiteSpace(x?.Value))
                        .WithMessage("Cvv je obavezno polje.")
                    .Must(x => Regex.IsMatch(x?.Value ?? "", @"^\d{3}$"))
                        .WithMessage("Cvv nije u ispravnom formatu.");

                _validator.RuleFor(x => x.Expiration)
                    .NotNull().WithMessage("Datum isteka kartice je obavezno polje.")
                    .Must(x => !string.IsNullOrWhiteSpace(x?.Value))
                        .WithMessage("Datum isteka kartice je obavezno polje.")
                    .Must(x =>
                    {
                        if (x == null || string.IsNullOrWhiteSpace(x.Value))
                            return false;

                        var valid = DateTime.TryParseExact(
                            "01/" + x.Value,
                            "dd/MM/yy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var expDate);

                        return valid && expDate > DateTime.Now;
                    })
                        .WithMessage("Neispravan datum ili je kartica istekla. Ispravan format: mm/gg.");
            });

            _validator.RuleFor(x => x.Notes)
                .Must(x => x == null || x.Value == null || x.Value.Length <= 150)
                .WithMessage("Maksimalni broj karaktera je 150.");
        }
        public ValidationResult Validate(CheckoutViewModel vm)
        {
            return _validator.Validate(vm);
        }
    }
}

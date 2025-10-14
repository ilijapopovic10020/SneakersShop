using FluentValidation;
using FluentValidation.Results;
using SneakersShop.Validators.Interfaces;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Implementations
{
    public class AuthViewModelValidator : IAuthViewModelValidator
    {
        private readonly AbstractValidator<AuthViewModel> _validator;

        public AuthViewModelValidator()
        {
            // FluentValidation rules
            _validator = new InlineValidator<AuthViewModel>();

            _validator.RuleFor(x => x.Username.Value)
                      .NotEmpty().WithMessage("Korisničko ime je obavezno.")
                      .Matches("^(?=[a-zA-Z0-9._]{4,30}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                      .WithMessage("Neispravan format korisničkog imena. Minimum 4, maksimum 30 karaktera, slova, brojevi i specijalni karakteri (.,_)");

            _validator.RuleFor(x => x.Password.Value)
                      .NotEmpty().WithMessage("Lozinka je obavezna.")
                      .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                      .WithMessage("Neispravan format lozinke");
        }

        public ValidationResult Validate(AuthViewModel vm)
        {
            return _validator.Validate(vm);
        }
    }
}

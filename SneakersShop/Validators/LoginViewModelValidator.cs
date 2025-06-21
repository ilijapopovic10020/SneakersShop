using FluentValidation;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Korisničko ime je obavezno.")
                                .Matches("^(?=[a-zA-Z0-9._]{4,30}$)(?!.*[_.]{2})[^_.].*[^_.]$")
                                .WithMessage("Neispravan format korisničkog imena. Minimum 4, maksimum 30 karaktera, slova, brojevi i specijalni karakteri (.,_)");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Lozinka je obavezna.")
                                     .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                                     .WithMessage("Neispravan format lozinke");
        }
    }
}

using FluentValidation;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.Validators
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno polje.")
                .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
                .WithMessage("Ime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više imena.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Prezime je obavezno polje.")
                .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
                .WithMessage("Prezime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više prezimena.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("E-adresa je obavezno polje.")
                                 .EmailAddress()
                                 .WithMessage("E-adresa nije u ispravnom formatu.");

            RuleFor(x => x.Phone).NotEmpty().WithMessage("Broj telefona je obavezno polje.")
                         .Matches(@"^06\d{7,8}$")
                         .WithMessage("Broj telefona mora početi sa 06 i imati ukupno 9 ili 10 cifara (npr. 0632389912).");
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using SneakersShop.Validators.Interfaces;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Implementations
{
    public class UpdateUserViewModelValidator : IUpdateUserViewModelValidator
    {
        private readonly AbstractValidator<ProfileUpdateViewModel> _validator;

        public UpdateUserViewModelValidator()
        {
            _validator = new InlineValidator<ProfileUpdateViewModel>();

            _validator.RuleFor(x => x.FirstName.Value)
                .NotEmpty().WithMessage("Ime je obavezno polje.")
                .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
                .WithMessage("Ime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više imena.");

            _validator.RuleFor(x => x.LastName.Value)
                .NotEmpty().WithMessage("Prezime je obavezno polje.")
                .Matches(@"^([A-ZŠĐČĆŽ][a-zšđčćž]{2,})(\s[A-ZŠĐČĆŽ][a-zšđčćž]{2,})*$")
                .WithMessage("Prezime mora početi velikim slovom, imati najmanje 3 slova i može sadržati više prezimena.");

            _validator.RuleFor(x => x.Email.Value).NotEmpty().WithMessage("E-adresa je obavezno polje.")
                                 .EmailAddress()
                                 .WithMessage("E-adresa nije u ispravnom formatu.");

            _validator.RuleFor(x => x.Phone.Value).NotEmpty().WithMessage("Broj telefona je obavezno polje.")
                         .Matches(@"^06\d{7,8}$")
                         .WithMessage("Broj telefona mora početi sa 06 i imati ukupno 9 ili 10 cifara (npr. 0632389912).");
        }

        public ValidationResult Validate(ProfileUpdateViewModel vm)
        {
            return _validator.Validate(vm);
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using SneakersShop.Validators.Interfaces;
using SneakersShop.ViewModels;

namespace SneakersShop.Validators.Implementations
{
    public class PasswordUpdateViewModelValidator : IPasswordUpdateViewModelValidator
    {
        private readonly AbstractValidator<PasswordUpdateViewModel> _validator;

        public PasswordUpdateViewModelValidator()
        {
            _validator = new InlineValidator<PasswordUpdateViewModel>();

            _validator.RuleFor(x => x.OldPassword.Value)
                      .NotEmpty().WithMessage("Stara lozinka je obavezna.")
                      .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                      .WithMessage("Neispravan format lozinke.");

            _validator.RuleFor(x => x.NewPassword.Value)
                      .NotEmpty().WithMessage("Nova lozinka je obavezna.")
                      .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                      .WithMessage("Neispravan format lozinke.");

            _validator.RuleFor(x => x.ConfirmNewPassword.Value)
                      .NotEmpty().WithMessage("Potvrda nove lozinke je obavezna.")
                      .Matches("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{4,}$")
                      .WithMessage("Neispravan format lozinke.")
                      .Equal(x => x.NewPassword.Value)
                      .WithMessage("Nova lozinka i potvrda lozinke moraju biti iste.");
        }

        public ValidationResult Validate(PasswordUpdateViewModel vm)
        {
            
            return _validator.Validate(vm);
        }
    }
}

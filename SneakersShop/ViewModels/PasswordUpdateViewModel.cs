using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Validators.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class PasswordUpdateViewModel : ObservableObject
    {
        private readonly IPasswordUpdateService _passwordService;
        private readonly IPasswordUpdateViewModelValidator _validator;

        public PasswordUpdateViewModel(IPasswordUpdateService passwordService, IPasswordUpdateViewModelValidator validator)
        {
            _passwordService = passwordService;
            _validator = validator;

            OldPassword.ValueChanged += (_) => Validate();
            NewPassword.ValueChanged += (_) => Validate();
            ConfirmNewPassword.ValueChanged += (_) => Validate();
        }

        [ObservableProperty]
        private BindableField<string> oldPassword = new ();

        [ObservableProperty]
        private BindableField<string> newPassword = new();

        [ObservableProperty]
        private BindableField<string> confirmNewPassword = new();

        [ObservableProperty]
        private bool isUpdateButtonEnabled = true;

        private void Validate()
        {
            var result = _validator.Validate(this);

            IsUpdateButtonEnabled = result.IsValid;

            OldPassword.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("OldPassword"))?.ErrorMessage ?? string.Empty;
            NewPassword.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("NewPassword"))?.ErrorMessage ?? string.Empty;
            ConfirmNewPassword.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("ConfirmNewPassword"))?.ErrorMessage ?? string.Empty;
        }

        [RelayCommand]
        private async Task UpdatePassword()
        {
            try
            {
                var passwordModel = new PasswordUpdateModel
                {
                    OldPassword = OldPassword.Value,
                    NewPassword = NewPassword.Value
                };

                var result = await _passwordService.UpdatePasswordAsync(passwordModel);

                if (result)
                {
                    OldPassword.Value = "";
                    NewPassword.Value = "";
                    ConfirmNewPassword.Value = "";

                    var popup = new MessagePopup("Uspeh", "Uspešno ste promenili lozinku.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
            }
            catch (TaskCanceledException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (HttpRequestException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }
    }
}

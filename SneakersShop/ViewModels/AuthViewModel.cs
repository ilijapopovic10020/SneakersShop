using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Validators.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly IAuthViewModelValidator _validator;

        public BindableField<string> Username { get; set; } = new();
        public BindableField<string> Password { get; set; } = new();

        [ObservableProperty]
        private bool isLoginButtonEnabled = true;

        public AuthViewModel(IAuthService authService, IAuthViewModelValidator validator)
        {
            Username.Value = "john_doe";
            Password.Value = "sifra123";

            _authService = authService;
            _validator = validator;

            Username.ValueChanged += (_) => Validate();
            Password.ValueChanged += (_) => Validate();
        }

        private void Validate()
        {
            var result = _validator.Validate(this);

            IsLoginButtonEnabled = result.IsValid;

            Username.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Username"))?.ErrorMessage ?? string.Empty;
            Password.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Password"))?.ErrorMessage ?? string.Empty;
        }

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                var auth = new AuthModel
                {
                    Username = Username.Value,
                    Password = Password.Value,
                    DeviceInfo = await DeviceInfoHelper.GetDeviceInfoAsync()
                };

                var authInfo = await _authService.LoginAsync(auth);

                if (authInfo != null)
                {
                    await SecureStorage.Default.SetAsync("user", JsonConvert.SerializeObject(authInfo));

                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}

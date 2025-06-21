using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SneakersShop.Components;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using SneakersShop.Validators;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginViewModelValidator _validator = new();
        private readonly UserService _userService = new();

        [ObservableProperty]
        private string? username = "john_doe";
        [ObservableProperty]
        private string? usernameError;
        public bool UsernameHasError => !string.IsNullOrEmpty(UsernameError);

        [ObservableProperty]
        private string? password = "sifra123";
        [ObservableProperty]
        private string? passwordError;
        public bool PasswordHasError => !string.IsNullOrEmpty(PasswordError);

        partial void OnUsernameChanged(string? value)
        {
            Validate();
            OnPropertyChanged(nameof(UsernameHasError));
        }
        partial void OnPasswordChanged(string? value)
        {
            Validate();
            OnPropertyChanged(nameof(PasswordHasError));
        }

        public LoginViewModel()
        {
            _userService = new();
            _validator = new();

            UsernameError = string.Empty;
            PasswordError = string.Empty;
        }

        private void Validate()
        {
            var result = _validator.Validate(this);

            UsernameError = result.Errors.FirstOrDefault(x => x.PropertyName == nameof(Username))?.ErrorMessage ?? string.Empty;
            PasswordError = result.Errors.FirstOrDefault(x => x.PropertyName == nameof(Password))?.ErrorMessage ?? string.Empty;
        }

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Username))
                {
                    throw new Exception("Korisničko ime je obavezno polje.");
                }

                if (string.IsNullOrEmpty(Password))
                {
                    throw new Exception("Lozinka je obavezno polje.");
                }

                LoginModel loginModel = await _userService.Login(Username, Password);
                

                if (loginModel != null)
                {
                    await SecureStorage.Default.SetAsync("user", JsonConvert.SerializeObject(loginModel));

                    if (Application.Current != null)
                    {
                        Application.Current.MainPage = new AppShell();
                    }
                }                
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}

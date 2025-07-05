using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using SneakersShop.Validators;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly LoginViewModelValidator _validator;

        public BindableField<string> Username { get; set; } = new();
        public BindableField<string> Password { get; set; } = new();

        [ObservableProperty]
        private bool isLoginbuttonEnabled = false;

        public LoginViewModel()
        {
            _userService = new();
            _validator = new();

            Username.ValueChanged += (_)  => Validate();
            Password.ValueChanged += (_) => Validate();
        }
        
        private void Validate()
        {
            var result = _validator.Validate(this);

           
            IsLoginbuttonEnabled = result.IsValid;

            
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

                LoginModel loginModel = await _userService.Login(auth);

                if (loginModel != null)
                {
                    await SecureStorage.Default.SetAsync("user", JsonConvert.SerializeObject(loginModel));

                    var cartItemsFromServer = await CartService.Instance.Get();
                    CartService.Instance.LoadFromServer(cartItemsFromServer);

                    if (Application.Current != null)
                    {
                        Application.Current.MainPage = new AppShell();
                    }
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

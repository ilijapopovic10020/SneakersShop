using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Validators.Interfaces;
using SneakersShop.Views;

namespace SneakersShop.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IRegisterViewModelValidator _validator;
        private readonly IAuthService _authService;

        public RegisterViewModel(IUserService userService, IRegisterViewModelValidator validator, IAuthService authService)
        {
            _userService = userService;
            _validator = validator;
            _authService = authService;

            FirstName.ValueChanged += (_) => ValidateStepOne();
            LastName.ValueChanged += (_) => ValidateStepOne();
            Phone.ValueChanged += (_) => ValidateStepOne();
            Email.ValueChanged += (_) => ValidateStepTwo();
            Username.ValueChanged += (_) => ValidateStepTwo();
            Password.ValueChanged += (_) => ValidateStepTwo();
        }        

        public BindableField<string> FirstName { get; set; } = new();
        public BindableField<string> LastName { get; set; } = new();
        public BindableField<string> Email { get; set; } = new();
        public BindableField<string> Username { get; set; } = new();
        public BindableField<string> Password { get; set; } = new();
        public BindableField<string> Phone { get; set; } = new();

        [ObservableProperty]
        private bool isNextStepEnabled = false;

        [ObservableProperty]
        private bool isRegisterButtonEnabled = false;

        [ObservableProperty]
        private bool isNextStepVisible = false;

        [ObservableProperty]
        private ImageSource displayedImageSource = ImageSource.FromUri(new Uri($"{AppConstants.IMAGE_URL}/images/profile/default.webp"));

        [ObservableProperty]
        private string image = string.Empty;

        private void ValidateStepOne()
        {
            var errors = _validator.Validate(this).Errors;

            var relevatErrors = errors.Where(x =>
                x.PropertyName.Contains("FirstName") ||
                x.PropertyName.Contains("LastName") ||
                x.PropertyName.Contains("Phone")).ToList();

            IsNextStepEnabled = relevatErrors.Count == 0;

            FirstName.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("FirstName"))?.ErrorMessage ?? "";

            LastName.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("LastName"))?.ErrorMessage ?? "";

            Phone.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("Phone"))?.ErrorMessage ?? "";
        }

        private void ValidateStepTwo()
        {

            var errors = _validator.Validate(this).Errors;

            var relevatErrors = errors.Where(x =>
                x.PropertyName.Contains("Email") ||
                x.PropertyName.Contains("Username") ||
                x.PropertyName.Contains("Password")).ToList();

            IsRegisterButtonEnabled = relevatErrors.Count == 0;

            Email.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("Email"))?.ErrorMessage ?? "";

            Username.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("Username"))?.ErrorMessage ?? "";

            Password.Error = relevatErrors.FirstOrDefault(x => x.PropertyName.Contains("Password"))?.ErrorMessage ?? "";
        }

        [RelayCommand]
        private async Task PickImage()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick a brand image."
                });

                if (result == null)
                {
                    var popup = new MessagePopup("Greška", "Slika nije selektovana, pokušajte ponovo.");

                    if (Application.Current != null && Application.Current.MainPage != null)
                        await Application.Current.MainPage.ShowPopupAsync(popup);
                    return;
                }


                using var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                Image = Convert.ToBase64String(memoryStream.ToArray());

                DisplayedImageSource = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Image)));
                OnPropertyChanged(nameof(DisplayedImageSource));
            }
            catch (Exception)
            {
                var popup = new MessagePopup("Greška", "Greška prilikom dohvatanja slike. Molimo pokušajte kasnije ili proverite da li je slika jpg ili png.");

                if (Application.Current != null && Application.Current.MainPage != null)
                    await Application.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                var user = new RegisterUserModel
                {
                    FirstName = FirstName.Value,
                    LastName = LastName.Value,
                    Email = Email.Value,
                    Username = Username.Value,
                    Password = Password.Value,
                    Phone = Phone.Value,
                    Image = Image
                };

                var result = await _userService.Register(user);

                if (result)
                {
                    var popup = new MessagePopup("Uspešno", "Registracija je uspešno završena.");
                    await App.Current.MainPage.ShowPopupAsync(popup);

                    try
                    {
                        var userAuth = new AuthModel
                        {
                            Username = Username.Value,
                            Password = Password.Value,
                            DeviceInfo = await DeviceInfoHelper.GetDeviceInfoAsync()
                        };

                        var authInfo = await _authService.LoginAsync(userAuth);

                        if (authInfo != null)
                        {
                            await SecureStorage.Default.SetAsync("user", JsonConvert.SerializeObject(authInfo));

                            Application.Current.MainPage = new AppShell();
                        }
                    }
                    catch
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private void GoToNextStep()
        {
            IsNextStepVisible = !IsNextStepVisible;
        }
    }
}

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Views;
using SneakersShop.Services;
using SneakersShop.Validators;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly RegisterViewModelValidator _validator;

        [ObservableProperty]
        private bool nextStepVisible = false;

        #region RegisterField
        [ObservableProperty]
        private string firstName = string.Empty;
        [ObservableProperty]
        private string? firstNameError;
        public bool FirstNameHasError => !string.IsNullOrEmpty(FirstNameError);

        [ObservableProperty]
        private string lastName = string.Empty;
        [ObservableProperty]
        private string? lastNameError;
        public bool LastNameHasError => !string.IsNullOrEmpty(LastNameError);

        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string? emailError;
        public bool EmailHasError => !string.IsNullOrEmpty(EmailError);

        [ObservableProperty]
        private string username = string.Empty;
        [ObservableProperty]
        private string? usernameError;
        public bool UsernameHasError => !string.IsNullOrEmpty(UsernameError);

        [ObservableProperty]
        private string password = string.Empty;
        [ObservableProperty]
        private string? passwordError;
        public bool PasswordHasError => !string.IsNullOrEmpty(PasswordError);

        [ObservableProperty]
        private string phone = string.Empty;
        [ObservableProperty]
        private string? phoneError;
        public bool PhoneHasError => !string.IsNullOrEmpty(PhoneError);

        [ObservableProperty]
        private string? image = string.Empty;
        [ObservableProperty]
        private string? imageName;
        #endregion

        [ObservableProperty]
        private bool isRegisterButtonEnabled = false;

        public RegisterViewModel()
        {
            _userService = new();
            _validator = new();

            FirstNameError = string.Empty;
            LastNameError = string.Empty;
            EmailError = string.Empty;
            UsernameError = string.Empty;
            PasswordError = string.Empty;
            PhoneError = string.Empty;
        }

        #region OnChanged
        partial void OnFirstNameChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(FirstNameHasError));
        }

        partial void OnLastNameChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(LastNameHasError));
        }

        partial void OnEmailChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(EmailHasError));
        }

        partial void OnUsernameChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(UsernameHasError));
        }

        partial void OnPasswordChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(PasswordHasError));
        }

        partial void OnPhoneChanged(string value)
        {
            Validate();
            OnPropertyChanged(nameof(PhoneHasError));
        }
        #endregion

        private void Validate()
        {
            try
            {
                var result = _validator.Validate(this);

                IsRegisterButtonEnabled = result.IsValid;

                FirstNameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("FirstName"))?.ErrorMessage ?? "";

                LastNameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("LastName"))?.ErrorMessage ?? "";

                EmailError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Email"))?.ErrorMessage ?? "";

                UsernameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Username"))?.ErrorMessage ?? "";

                PasswordError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Password"))?.ErrorMessage ?? "";

                PhoneError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Phone"))?.ErrorMessage ?? "";
            }
            finally
            {
                IsRegisterButtonEnabled = true;
            }
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
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                
                if(Application.Current != null && Application.Current.MainPage != null)
                    await Application.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                var user = new RegisterModel
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Username = Username,
                    Password = Password,
                    Phone = Phone,
                    Image = Image
                };

                var result = await _userService.Register(user);

                if (result)
                {
                    var popup = new MessagePopup("Uspešno", "Registracija je uspešno završena.");
                    await App.Current.MainPage.ShowPopupAsync(popup);

                    if (Application.Current != null)
                        Application.Current.MainPage = new LoginPage();

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
            NextStepVisible = !NextStepVisible;
        }
    }
}

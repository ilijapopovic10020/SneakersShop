using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using SneakersShop.Validators;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class UpdateUserViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly UpdateUserViewModelValidator _validator;

        [ObservableProperty]
        private UserModel user;

        [ObservableProperty]
        private bool isUpdateButtonEnabled;

        [ObservableProperty]
        private ImageSource displayedImageSource;

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
        #endregion

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

        public UpdateUserViewModel()
        {
            _userService = new();
            _validator = new();
        }

        private void Validate()
        {
            try
            {
                var result = _validator.Validate(this);

                IsUpdateButtonEnabled = result.IsValid;

                FirstNameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("FirstName"))?.ErrorMessage ?? "";

                LastNameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("LastName"))?.ErrorMessage ?? "";

                EmailError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Email"))?.ErrorMessage ?? "";

                UsernameError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Username"))?.ErrorMessage ?? "";

                PasswordError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Password"))?.ErrorMessage ?? "";

                PhoneError = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Phone"))?.ErrorMessage ?? "";
            }
            finally
            {
                IsUpdateButtonEnabled = true;
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
        private async Task UpdateUser()
        {
            try
            {
                var userToUpdate = new UserUpdateModel()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Image = Image,
                    Phone = Phone
                };

                var result = await _userService.Put(userToUpdate);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Uspešno se izmenili profil.");

                    if (Application.Current != null && Application.Current.MainPage != null)
                        await Application.Current.MainPage.ShowPopupAsync(popup);

                    await Application.Current.MainPage.Navigation.PopAsync();
                }

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);

                if (Application.Current != null && Application.Current.MainPage != null)
                    await Application.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}

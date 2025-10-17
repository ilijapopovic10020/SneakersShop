using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Extensions;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Validators.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class ProfileUpdateViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IUpdateUserViewModelValidator _validator;

        public ProfileUpdateViewModel(IUserService userService, IUpdateUserViewModelValidator validator)
        {
            _userService = userService;
            _validator = validator;

            FirstName.ValueChanged += (_) => Validate();
            LastName.ValueChanged += (_) => Validate();
            Phone.ValueChanged += (_) => Validate();
            Email.ValueChanged += (_) => Validate();
        }

        [ObservableProperty]
        private bool isUpdateButtonEnabled;

        public BindableField<string?> FirstName { get; set; } = new();
        public BindableField<string?> LastName { get; set; } = new();
        public BindableField<string?> Email { get; set; } = new();
        public BindableField<string?> Phone { get; set; } = new();

        [ObservableProperty]
        private string? image = string.Empty;

        [ObservableProperty]
        private ImageSource displayedImageSource;

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        private async Task LoadUser()
        {
            try
            {
                IsLoading = true;

                var user = await SecureStorage.Default.GetUser();
                var userInfo = await _userService.GetUserById(user.Id);

                FirstName.Value = userInfo.FirstName;
                LastName.Value = userInfo.LastName;
                Email.Value = userInfo.Email;
                Phone.Value = userInfo.Phone;

                DisplayedImageSource = ImageSource.FromUri(new Uri(userInfo.FullImageUrl));
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
            finally
            {
                IsLoading = false;
            }
        }

        private void Validate()
        {
            try
            {
                var result = _validator.Validate(this);

                IsUpdateButtonEnabled = result.IsValid;

                FirstName.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("FirstName"))?.ErrorMessage ?? "";

                LastName.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("LastName"))?.ErrorMessage ?? "";

                Email.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Email"))?.ErrorMessage ?? "";

                Phone.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Phone"))?.ErrorMessage ?? "";
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
                    await Shell.Current.ShowPopupAsync(popup);

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

                    await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task UpdateUser()
        {
            try
            {
                var userToUpdate = new UpdateUserModel()
                {
                    FirstName = FirstName.Value,
                    LastName = LastName.Value,
                    Email = Email.Value,
                    Image = Image,
                    Phone = Phone.Value
                };

                var result = await _userService.Update(userToUpdate);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Uspešno se izmenili profil.");
                        await Shell.Current.ShowPopupAsync(popup);

                    await Shell.Current.Navigation.PopAsync();
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

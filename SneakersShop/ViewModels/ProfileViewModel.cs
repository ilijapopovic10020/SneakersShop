using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    { 
        private readonly IUserService _userService;

        public ProfileViewModel(IUserService userService)
        {
            _userService = userService;

            User = new UserModel();
        }

        [ObservableProperty]
        private UserModel user;

        [ObservableProperty]
        private bool isUserLoggedIn;

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        private async Task LoadUser()
        {
            try
            {
                IsLoading = true;

                var user = await SecureStorage.Default.GetUser();

                User = await _userService.GetUserById(user.Id);

                IsUserLoggedIn = true;
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
            catch
            {
                if (User != null)
                    SecureStorage.Default.Remove("user");

                IsUserLoggedIn = false;
                IsLoading = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

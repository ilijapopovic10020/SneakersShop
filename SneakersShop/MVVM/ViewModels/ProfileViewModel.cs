using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private AppVisualTheme selectedTheme = AppVisualTheme.Light;

        [ObservableProperty]
        private UserModel user;

        [ObservableProperty]
        private bool isUserLoggedIn = false;

        [ObservableProperty]
        private bool isLoading = true;

        public ProfileViewModel()
        {
            _userService = new UserService();
            _ = LoadSavedThemeAsync();
        }

        [RelayCommand]
        private async Task LoadUser()
        {
            try
            {
                IsLoading = true;

                var user = await SecureStorage.Default.GetUser();
                User = await _userService.Get(user.Id);

                IsUserLoggedIn = true;
            }
            catch (Exception)
            {
                IsUserLoggedIn = false;
                IsLoading = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectTheme(AppVisualTheme theme)
        {
            await ThemeManager.SaveTheme(theme);
            SelectedTheme = theme;
        }

        private async Task LoadSavedThemeAsync()
        {
            SelectedTheme = await ThemeManager.LoadAndApplySavedThemeAsync();
        }
    }
}

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class FavoritesViewModel : ObservableObject
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesViewModel(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;

            Favorites = [];
        }

        [ObservableProperty]
        private ObservableCollection<ProductsModel> favorites;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int itemsPerPage = 8;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private bool isPaginationAvailable = true;

        [ObservableProperty]
        private bool isNextPageAvailable = true;

        [ObservableProperty]
        private bool isPreviousPageAvailable = true;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string? keyword;

        [ObservableProperty]
        private bool hasFavorites;

        [RelayCommand]
        private async Task LoadFavorites()
        {
            try
            {
                IsLoading = true;

                var favorites = await _favoriteService.GetFavoriteAsync(
                    CurrentPage,
                    ItemsPerPage,
                    Keyword
                );

                if (favorites.Data != null)
                {
                    Favorites = [.. favorites.Data];

                    TotalCount = favorites.TotalCount;
                    PagesCount = favorites.PagesCount;

                    IsPaginationAvailable = TotalCount > 0 && PagesCount > 1;

                    IsPreviousPageAvailable = CurrentPage > 1;

                    IsNextPageAvailable = CurrentPage != PagesCount;

                    HasFavorites = TotalCount > 0;
                }
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

        [RelayCommand]
        private async Task NextPage()
        {
            if (CurrentPage < PagesCount)
            {
                CurrentPage++;
                await LoadFavorites();
            }
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadFavorites();
            }
        }
    }
}

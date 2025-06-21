using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class FavoriteViewModel : ObservableObject
    {
        private readonly FavoriteService _favoriteService;

        [ObservableProperty]
        private ObservableCollection<ProductsModel> favorites;

        [ObservableProperty]
        private bool isLoading = true;

        [ObservableProperty]
        private bool isPaginationVisible = true;

        [ObservableProperty]
        private bool hasProducts = true;

        [ObservableProperty]
        private bool isScrolled = false;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private string? keyword;

        public FavoriteViewModel()
        {
            _favoriteService = new FavoriteService();

            Favorites = [];
        }

        [RelayCommand]
        private async Task LoadFavorites()
        {
            try
            {
                IsLoading = true;

                var search = new PagedSearchKw
                {
                    Keyword = Keyword,
                    Page = 1,
                    PerPage = 10
                };

                var favorites = await _favoriteService.Get(search);

                if(favorites.Data != null)
                    Favorites = [.. favorites.Data];

                IsPaginationVisible = TotalCount > 0 && PagesCount > 1;

                HasProducts = TotalCount > 0;
            }
            catch (Exception)
            {

                throw;
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

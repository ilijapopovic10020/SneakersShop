using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;

namespace SneakersShop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IBrandService _brandService;
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private ObservableCollection<BrandsModel> brands = [];

        [ObservableProperty]
        private ObservableCollection<CategoriesModel> categories = [];

        [ObservableProperty]
        private ObservableCollection<ProductsModel> bestSeller = [];

        [ObservableProperty]
        private ObservableCollection<ProductsModel> bestRated = [];

        [ObservableProperty]
        private ObservableCollection<ProductsModel> newCollection = [];

        [ObservableProperty]
        private ObservableCollection<ProductModel> recommended = [];

        [ObservableProperty]
        private bool isRecommendedVisible;

        [ObservableProperty]
        private bool isRecommendedLoading;

        [ObservableProperty]
        private bool isLoading;

        public HomeViewModel(
            IBrandService brandService,
            IProductsService productsService,
            ICategoryService categoryService
        )
        {
            _brandService = brandService;
            _productsService = productsService;
            _categoryService = categoryService;

            _ = InitializeCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task Initialize()
        {
            try
            {
                IsLoading = true;

                await LoadBrandsCategories();
                await LoadProducts();
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task LoadBrandsCategories()
        {
            try
            {
                var brands = await _brandService.GetBrandsAsync();
                var categories = await _categoryService.GetCategoriesAsync();

                Brands = [.. brands];
                Categories = [.. categories];
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                BestSeller = await LoadFromCacheOrFetch(
                    (int)FilterModel.BestSeller,
                    nameof(FilterModel.BestSeller)
                );
                BestRated = await LoadFromCacheOrFetch(
                    (int)FilterModel.BestRated,
                    nameof(FilterModel.BestRated)
                );
                NewCollection = await LoadFromCacheOrFetch(
                    (int)FilterModel.Newest,
                    nameof(FilterModel.Newest)
                );
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        private async Task<ObservableCollection<ProductsModel>> LoadFromCacheOrFetch(
            int filter,
            string key
        )
        {
            var duration = TimeSpan.FromHours(24);

            var cached = await LocalCacheService.GetAsync<List<ProductsModel>>(key, duration);

            if (cached != null)
                return [.. cached];

            var search = new ProductsSearch
            {
                Page = 1,
                PerPage = 6,
                Filter = filter,
            };
            var result = await _productsService.GetProductsAsync(search);
            var list = (result.Data ?? []).ToList();

            await LocalCacheService.SaveAsync(key, list);

            return [.. list];
        }

        [RelayCommand]
        private async Task LoadRecommended()
        {
            try
            {
                IsRecommendedLoading = true;

                var recommended = await LastCheckedProductsCache.GetLastCheckedProducts();

                if (recommended == null || recommended.Count <= 0)
                {
                    Recommended = [];
                    IsRecommendedVisible = false;
                }
                else
                {
                    Recommended = [.. recommended];
                    IsRecommendedVisible = true;
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
            finally
            {
                IsRecommendedLoading = false;
            }
        }
    }
}

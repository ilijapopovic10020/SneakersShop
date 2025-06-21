using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    internal partial class HomeViewModel : ObservableObject
    {
        private readonly BrandService _brandService;
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        [ObservableProperty]
        private ObservableCollection<BrandModel> brands;
        [ObservableProperty]
        private ObservableCollection<CategoryModel> categories;

        [ObservableProperty]
        private string keyword;

        [ObservableProperty]
        private ObservableCollection<ProductsModel> bestSeller; //most popular
        
        [ObservableProperty]
        private ObservableCollection<ProductsModel> bestRated; //best seller

        [ObservableProperty]
        private ObservableCollection<ProductsModel> newCollection; // new collection

        [ObservableProperty]
        private bool isLoading = false;

        public HomeViewModel()
        {
            _brandService = new BrandService();
            _categoryService = new CategoryService();
            _productService = new ProductService();

            Brands = [];
            BestSeller = [];
            BestRated = [];
            NewCollection = [];
            Categories = [];

            LoadData();
        }

        public async void LoadData()
        {
            await LocalCacheService.InitAsync();
            IsLoading = true;
            
            try
            {
                var brands = await _brandService.Get();
                var categories = await _categoryService.Get();

                Brands = [.. brands];
                Categories = [.. categories];

                BestSeller = await LoadFromCacheOrFetch("BestSeller", Filter.BestSeller);
                BestRated = await LoadFromCacheOrFetch("BestRated", Filter.BestRated);
                NewCollection = await LoadFromCacheOrFetch("NewCollection", Filter.Newest);

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task<ObservableCollection<ProductsModel>> LoadFromCacheOrFetch(string key, Filter filter)
        {
            var duration = TimeSpan.FromHours(24);

            var cached = await LocalCacheService.GetAsync<List<ProductsModel>>(key, duration);
            if (cached != null)
                return new ObservableCollection<ProductsModel>(cached);

            var search = new ProductSearch { Page = 1, PerPage = 6, Filter = (int)filter };
            var result = await _productService.Get(search);
            var list = (result.Data ?? []).ToList();
            await LocalCacheService.SaveAsync(key, list);
            return new ObservableCollection<ProductsModel>(list);
        }

    }
}

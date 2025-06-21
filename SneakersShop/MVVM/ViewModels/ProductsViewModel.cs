using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        #region Services

        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;

        private readonly ColorService _colorService;

        private readonly BrandService _brandService;

        #endregion

        #region Filtering and sorting

        [ObservableProperty]
        private ObservableCollection<FilterOption> filters;

        [ObservableProperty]
        private FilterOption selectedFilter;

        [ObservableProperty]
        private ObservableCollection<CategoryModel> categories;

        [ObservableProperty]
        private CategoryModel selectedCategory;

        [ObservableProperty]
        private ObservableCollection<ColorModel> colors;

        [ObservableProperty]
        private ColorModel? selectedColor;

        [ObservableProperty]
        private bool isShowingAllColors = false;

        [ObservableProperty]
        private ObservableCollection<ColorModel> visibleColors;

        [ObservableProperty]
        private ObservableCollection<BrandModel> brands;

        [ObservableProperty]
        private BrandModel? selectedBrand;

        [ObservableProperty]
        private string? keyword;

        [ObservableProperty]
        private int minPrice = 0;

        [ObservableProperty]
        private int maxPrice = 30000;

        [ObservableProperty]
        private bool isFilterVisible = true;

        #endregion

        #region Pagination
        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool isPaginationVisible = true;

        [ObservableProperty]
        private bool hasProducts = true;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private int totalCount;
        #endregion

        [ObservableProperty]
        private ObservableCollection<ProductsModel> products;

        [ObservableProperty]
        private bool isScrolled = false;

        public ProductsViewModel()
        {
            _productService = new();
            _categoryService = new();
            _colorService = new();
            _brandService = new();

            Products = [];
            Categories = [];
            Colors = [];
            Brands = [];

            Filters = [.. Enum.GetValues(typeof(Filter))
                    .Cast<Filter>()
                    .Select(f => new FilterOption { Filter = f })];

            SelectedFilter = Filters.FirstOrDefault();
            SelectedFilter.IsSelected = true;
        }

        [RelayCommand]
        private async Task LoadFilters()
        {
            try
            {
                var brands = await _brandService.Get();

                if (brands != null)
                {
                    Brands = new ObservableCollection<BrandModel>(brands);
                }

                var categories = await _categoryService.Get();

                if (categories != null)
                {
                    Categories = new ObservableCollection<CategoryModel>(categories);
                    if (Categories.Any())
                    {
                        SelectedCategory = Categories.First();
                    }
                }

                var colors = await _colorService.Get();

                if (colors != null)
                {
                    Colors = new ObservableCollection<ColorModel>(colors.Where(color => !color.Name.Contains('-')));
                    UpdateVisibleColors();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                IsLoading = true;

                var selectedColors = Colors.Where(color => color.IsSelected).Select(color => color.Name).ToList();

                var selectedBrands = Brands.Where(brand => brand.IsSelected).Select(brand => brand.Id).ToList();

                var search = new ProductSearch
                {
                    Keyword = Keyword,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice,
                    Page = CurrentPage,
                    PerPage = 10,
                    Filter = (int)SelectedFilter.Filter,
                    BrandId = selectedBrands.Count > 0 ? selectedBrands : null,
                    Color = selectedColors.Count > 0 ? selectedColors : null,
                    CategoryId = SelectedCategory?.Id,
                };

                var products = await _productService.Get(search);

                if (products.Data != null)
                    Products = [.. products.Data];

                TotalCount = products.TotalCount;
                CurrentPage = products.CurrentPage;
                PagesCount = products.PagesCount;

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
        private void ToggleColorVisibility()
        {
            IsShowingAllColors = !IsShowingAllColors;
            UpdateVisibleColors();
        }

        [RelayCommand]
        private async Task Search()
        {
            foreach (var brand in Brands)
                brand.IsSelected = false;

            foreach (var color in Colors)
                color.IsSelected = false;

            foreach (var category in Categories)
                category.IsSelected = false;

            CurrentPage = 1;
            IsShowingAllColors = false;
            SelectedFilter = Filters.FirstOrDefault();

            await LoadProducts();
            await LoadFilters();
        }

        [RelayCommand]
        private async Task ApplyFilters()
        {
            CurrentPage = 1;
            await LoadProducts();
        }

        [RelayCommand]
        private async Task ClearFilters()
        {
            foreach (var brand in Brands)
                brand.IsSelected = false;

            foreach (var color in Colors)
                color.IsSelected = false;

            foreach (var category in Categories)
                category.IsSelected = false;

            Keyword = string.Empty;
            CurrentPage = 1;
            IsShowingAllColors = false;

            await LoadProducts();
            await LoadFilters();
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (CurrentPage < PagesCount)
            {
                CurrentPage++;
                await LoadProducts();
            }
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadProducts();
            }
        }

        [RelayCommand]
        private async Task ShowSortingOptions()
        {
            var popup = new FilterPopup(Filters, SelectedFilter);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is FilterOption selected)
            {
                SelectedFilter = selected;
                SelectedFilter.IsSelected = true;
                await LoadProducts();
            }
        }

        private void UpdateVisibleColors()
        {
            if (IsShowingAllColors)
            {
                VisibleColors = new ObservableCollection<ColorModel>(Colors);
            }
            else
            {
                VisibleColors = new ObservableCollection<ColorModel>(Colors.Take(5));
            }
        }
    }
}

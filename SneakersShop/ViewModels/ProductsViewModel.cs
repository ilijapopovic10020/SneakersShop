using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(MinPriceString), "MinPrice")]
    [QueryProperty(nameof(MaxPriceString), "MaxPrice")]
    [QueryProperty(nameof(SelectedBrandsString), "SelectedBrands")]
    [QueryProperty(nameof(SelectedColorsString), "SelectedColors")]
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private ObservableCollection<ProductsModel> products;

        #region Pagination
        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int itemsPerPage = 15;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private bool isPageinationAvailable = true;

        [ObservableProperty]
        private bool isNextPageAvailable = true;

        [ObservableProperty]
        private bool isPreviousPageAvailable = true;

        #endregion

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private ObservableCollection<FilterOption> filters;

        [ObservableProperty]
        private FilterOption selectedFilter;

        [ObservableProperty]
        private ObservableCollection<CategoriesModel> categories;

        [ObservableProperty]
        private CategoriesModel? selectedCategory;

        [ObservableProperty]
        private string? keyword;

        [ObservableProperty]
        private string? minPriceString;
        public decimal? MinPrice => decimal.TryParse(MinPriceString, out var result) ? result : null;

        [ObservableProperty]
        private string? maxPriceString;

        private decimal? MaxPrice => decimal.TryParse(MaxPriceString, out var result) ? result : null;

        [ObservableProperty]
        private string? selectedBrandsString;

        private List<int>? SelectedBrands => !string.IsNullOrWhiteSpace(SelectedBrandsString) 
            ? [.. SelectedBrandsString.Split(',').Select(int.Parse)]
            : null;

        [ObservableProperty]
        private string? selectedColorsString;

        public List<string>? SelectedColors => !string.IsNullOrWhiteSpace(SelectedColorsString) 
            ? [.. SelectedColorsString.Split(',')]
            : null;

        public ProductsViewModel(IProductsService productsService, ICategoryService categoryService)
        {
            _productsService = productsService;
            _categoryService = categoryService;

            Products = [];
            Categories = [];

            Filters = [.. Enum.GetValues(typeof(FilterModel))
                        .Cast<FilterModel>()
                        .Select(f => new FilterOption{Filter = f})];

            SelectedFilter = Filters.FirstOrDefault();
            SelectedFilter.IsSelected = true;

            _ = LoadCategories();
        }

        private async Task LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();

                if (categories != null)
                {
                    var allCategories = new List<CategoriesModel>
                    {
                        new() { Id = 0, Name = "Sve kategorije", IsSelected = false }
                    };
                    allCategories.AddRange(categories);

                    Categories = [.. allCategories];

                    SelectedCategory = allCategories.First();
                    SelectedCategory.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                IsLoading = true;

                var search = new ProductsSearch()
                {
                    Page = CurrentPage,
                    PerPage = ItemsPerPage,
                    Filter = (int)SelectedFilter.Filter,
                    Keyword = Keyword,
                    CategoryId = SelectedCategory?.Id == 0 ? null : SelectedCategory?.Id,
                    BrandId = SelectedBrands,
                    Color = SelectedColors,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice
                };

                var products = await _productsService.GetProductsAsync(search);

                if (products.Data != null)
                    Products = [.. products.Data];

                TotalCount = products.TotalCount;
                PagesCount = products.PagesCount;

                if (PagesCount == 1)
                    IsPageinationAvailable = false;
                else
                    IsPageinationAvailable = true;

                if (CurrentPage == 1)
                    IsPreviousPageAvailable = false;
                else
                    IsPreviousPageAvailable = true;

                if (CurrentPage == PagesCount)
                    IsNextPageAvailable = false;
                else
                    IsNextPageAvailable = true;
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

        [RelayCommand]
        private async Task ShowSortOptions()
        {
            var popup = new SortPopup(Filters, SelectedFilter);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if(result is FilterOption selected)
            {
                SelectedFilter = selected;
                SelectedFilter.IsSelected = true;

                CurrentPage = 1;
                await LoadProductsCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task ShowCategoryOptions()
        {
            var popup = new CategoryPopup(Categories, SelectedCategory);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is CategoriesModel selected)
            {
                SelectedCategory = selected;
                SelectedCategory.IsSelected = true;

                CurrentPage = 1;
                await LoadProductsCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private async Task Search()
        {
            SelectedBrandsString = null;
            SelectedColorsString = null;
            MinPriceString = null;
            MaxPriceString = null;
            CurrentPage = 1;
            SelectedCategory = null;
            await LoadProducts();
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
    }
}

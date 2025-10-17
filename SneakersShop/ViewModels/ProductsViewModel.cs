using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    
    public partial class ProductsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IProductsService _productsService;
        private readonly ICategoryService _categoryService;

        public ProductsViewModel(IProductsService productsService, ICategoryService categoryService)
        {
            _productsService = productsService;
            _categoryService = categoryService;

            Filters = [.. Enum.GetValues(typeof(FilterModel))
                        .Cast<FilterModel>()
                        .Select(f => new FilterOption{Filter = f})];

            SelectedFilter = Filters.FirstOrDefault();
            SelectedFilter.IsSelected = true;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            if (query.TryGetValue("Keyword", out var keyword))
                Keyword = keyword as string ?? string.Empty;

            if (query.TryGetValue("SelectedCategory", out var category))
                SelectedCategory = category as CategoriesModel ?? new CategoriesModel();            

            if(query.TryGetValue("SelectedBrands", out var brands))
                SelectedBrands = brands as List<int> ?? [];

            if (query.TryGetValue("SelectedColors", out var colors))
                SelectedColors = colors as List<string> ?? [];

            if (query.TryGetValue("MinPrice", out var minPrice))
                MinPrice = (decimal?)(minPrice ?? null);

            if (query.TryGetValue("MaxPrice", out var maxPrice))
                MaxPrice = (decimal?)(maxPrice ?? null);
        }

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private ObservableCollection<ProductsModel> products = [];

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

        #region Sorting
        [ObservableProperty]
        private ObservableCollection<FilterOption> filters;

        [ObservableProperty]
        private FilterOption selectedFilter;
        #endregion

        #region Filtering
        [ObservableProperty]
        private ObservableCollection<CategoriesModel> categories = [];

        [ObservableProperty]
        private CategoriesModel? selectedCategory = new();

        [ObservableProperty]
        private string? keyword;

        [ObservableProperty]
        private List<int>? selectedBrands = [];

        [ObservableProperty]
        private List<string>? selectedColors = [];

        [ObservableProperty]
        private decimal? minPrice;

        [ObservableProperty]
        private decimal? maxPrice;
        #endregion


        [RelayCommand]
        private async Task LoadCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();

                var allCategories = new List<CategoriesModel>
                    {
                        new() { Id = 0, Name = "Sve kategorije", IsSelected = false }
                    };
                allCategories.AddRange(categories);

                Categories = [.. allCategories];

                SelectedCategory = Categories.First();
                SelectedCategory.IsSelected = true;
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

        [RelayCommand]
        private async Task LoadProducts()
        {
            try
            {
                IsLoading = true;
                var search = new ProductsSearch
                {
                    Page = CurrentPage,
                    PerPage = ItemsPerPage,
                    Keyword = Keyword,
                    CategoryId = SelectedCategory?.Id == 0 ? null : SelectedCategory?.Id,
                    BrandId = SelectedBrands.Count != 0 ? SelectedBrands : null,
                    Color = SelectedColors.Count != 0 ? SelectedColors : null,
                    MinPrice = MinPrice,
                    MaxPrice = MaxPrice,
                    Filter = (int)SelectedFilter.Filter
                };

                var products = await _productsService.GetProductsAsync(search);

                Products = [.. products.Data];
                TotalCount = products.TotalCount;
                PagesCount = products.PagesCount;
                CurrentPage = products.CurrentPage;

                IsPageinationAvailable = TotalCount > 0;
                IsNextPageAvailable = CurrentPage < PagesCount;
                IsPreviousPageAvailable = CurrentPage > 1;
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
                var popup = new MessagePopup("Greška", ex.InnerException.Message);
                await Shell.Current.ShowPopupAsync(popup);
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

            if (result is FilterOption selected)
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
            SelectedBrands = [];
            SelectedColors = [];
            MinPrice = null;
            MaxPrice= null;
            CurrentPage = 1;
            SelectedCategory.IsSelected = false;
            SelectedCategory = Categories.FirstOrDefault(x => x.Id == 0);
            SelectedCategory.IsSelected = true;
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

        [RelayCommand]
        private async Task FiltersOptions()
        {
            await Shell.Current.GoToAsync($"{nameof(FiltersPage)}", true, new Dictionary<string, object?>
            {
                { "SelectedBrands", SelectedBrands },
                { "SelectedColors", SelectedColors },
                { "MinPrice", MinPrice },
                { "MaxPrice", MaxPrice }
            });
        }
    }
}
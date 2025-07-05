using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.MVVM.Views;
using SneakersShop.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        private readonly ProductService _productService;
        private readonly FavoriteService _favoriteService;
        private readonly ReviewService _reviewService;

        [ObservableProperty]
        private ProductModel product;

        [ObservableProperty]
        private ObservableCollection<SizeModel> sizes;

        [ObservableProperty]
        private ObservableCollection<ReviewsModel> reviews;

        [ObservableProperty]
        private ObservableCollection<ProductModel> recent;

        [ObservableProperty]
        private bool isRecentVisible = false;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private int currentImageIndex;

        [ObservableProperty]
        private SizeModel selectedSize;

        [ObservableProperty]
        private string favoriteImage;

        partial void OnCurrentImageIndexChanged(int value)
        {
            UpdateImageInidactor();
        }

        [ObservableProperty]
        private string? previousImage;

        [ObservableProperty]
        private string? currentImage;

        [ObservableProperty]
        private string? nextImage;

        public ProductViewModel()
        {
            _productService = new ProductService();
            _favoriteService = new FavoriteService();
            _reviewService = new ReviewService();

            Product = new ProductModel();
            Sizes = [];
            Reviews = [];
            Recent = [];
        }

        [RelayCommand]
        public async Task LoadProduct(int id)
        {
            try
            {
                IsLoading = true;

                if(Product.Id != id)
                    Product = await _productService.Get(id);

                if(Product.Sizes != null)
                    Sizes = [.. Product.Sizes];

                if (Product.IsFavorite)
                {
                    FavoriteImage = "heart_selected.svg";
                }
                else
                {
                    FavoriteImage = "heart.svg";
                }

                if (Product?.Variants != null && Product.Variants.Any())
                {
                    foreach (var variant in Product.Variants)
                    {
                        variant.IsSelected = variant.Id == id;
                    }
                }
                CurrentImageIndex = 0;
                UpdateImageInidactor();
                await LoadRecent();

                var reviewSearch = new PagedSearchId
                {
                    Id = id,
                    Page = 1,
                    PerPage = 6,
                };
                var reviews = await _reviewService.Get(reviewSearch);

                if (reviews != null && reviews.Data != null)
                {
                    Reviews = [.. reviews.Data];
                }
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
        private async Task ChangeVaraint(int id)
        {
            try
            {

                Product = await _productService.Get(id);

                await LastCheckedProductsCache.SaveLastCheckedProducts(Product);

                await LoadProduct(id);

            }
            catch (Exception)
            {

                throw;
            }

        }

        [RelayCommand]
        private void UpdateImageInidactor()
        {
            if (Product?.FullImageUrls == null || Product.FullImageUrls.Count == 0)
                return;
            var count = Product.FullImageUrls.Count;

            PreviousImage = Product.FullImageUrls[(CurrentImageIndex - 1 + count) % count];
            CurrentImage = Product.FullImageUrls[CurrentImageIndex % count];
            NextImage = Product.FullImageUrls[(CurrentImageIndex + 1) % count];
        }

        [RelayCommand]
        private async Task Favorite()
        {
            if (Product.IsFavorite)
            {
                FavoriteImage = "heart.svg";
                Product.IsFavorite = !Product.IsFavorite;
                await _favoriteService.Delete(Product.Id);
            }
            else
            {
                FavoriteImage = "heart_selected.svg";
                Product.IsFavorite = !Product.IsFavorite;
                await _favoriteService.Post(Product.Id);
            }
        }

        [RelayCommand]
        private async Task SelectSize(int id)
        {
            var size = Sizes.FirstOrDefault(x => x.Id == id);

            if (size == null) 
            {
                await SnackbarHelper.ShowMessage("Došlo je do greške, molimo pokušajte ponovo.");
            }
            else
            {
                if(SelectedSize != null)
                    SelectedSize.IsSelected = false;

                size.IsSelected = true;
                SelectedSize = size;
                Sizes = [.. Sizes];
            }
        }

        [RelayCommand]
        public async Task BuyNow()
        {
            if(SelectedSize == null)
            {
                await SnackbarHelper.ShowMessage("Molimo vas izaberite velicinu pre nego što dodate proizvod u korpu.");
            }
            else
            {
                await CartService.Instance.AddItem(new CartModel
                {
                    Product = Product,
                    Size = SelectedSize,
                    Quantity = 1,
                });

                await Shell.Current.GoToAsync($"//{nameof(CartPage)}");

            }
        }

        [RelayCommand]
        public async Task AddToCart()
        {
            try
            {
                if (SelectedSize == null)
                {
                    await SnackbarHelper.ShowMessage("Molimo vas izaberite velicinu pre nego što dodate proizvod u korpu.");
                }
                else
                {
                    await CartService.Instance.AddItem(new CartModel
                    {
                        Product = Product,
                        Size = SelectedSize,
                        Quantity = 1,
                    });

                    //await SnackbarHelper.ShowMessage("Proizvod je dodat u korpu");

                    if (Application.Current != null && Application.Current.MainPage != null)
                        await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        private async Task LoadRecent()
        {
            var recent =  await LastCheckedProductsCache.GetLastCheckedProducts();

            if (recent == null || recent.Count <= 0)
            {
                Recent = [];
                IsRecentVisible = false;
            }
            else
            {
                Recent = [.. recent];
                IsRecentVisible = true;
            }
        }
    }
}

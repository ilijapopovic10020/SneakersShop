using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Exceptions;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(ProductId), "ProductId")]
    public partial class ProductViewModel(IProductsService productsService, 
                                          IReviewService reviewService, 
                                          IFavoriteService favoriteService,
                                          ICartService cartService) : ObservableObject
    {
        [ObservableProperty]
        private int productId;

        private readonly IProductsService _productsService = productsService;
        private readonly IReviewService _reviewService = reviewService;
        private readonly IFavoriteService _favoriteService = favoriteService;
        private readonly ICartService _cartService = cartService;

        [ObservableProperty]
        private ProductModel product = new();

        [ObservableProperty]
        private ObservableCollection<SizeModel> sizes = [];

        [ObservableProperty]
        private SizeModel? selectedSize;

        [ObservableProperty]
        private ObservableCollection<ProductModel> recent = [];

        [ObservableProperty]
        private bool isRecentVisible = false;

        [ObservableProperty]
        private ObservableCollection<ReviewsModel> reviews = [];    

        [ObservableProperty]
        private bool isReviewsVisible = false;

        [ObservableProperty]
        private int currentImageIndex;

        [ObservableProperty]
        private string? previousImage;

        [ObservableProperty]
        private string? currentImage;

        [ObservableProperty]
        private string? nextImage;

        [ObservableProperty]
        private bool isLoading = false;

        partial void OnCurrentImageIndexChanged(int value)
        {
            UpdateImageInidactor();
        }

        [RelayCommand]
        public async Task LoadProduct()
        {
            try
            {
                IsLoading = true;

                if (Product.Id != ProductId)
                {
                    Product = await _productsService.GetProductByIdAsync(ProductId);
                    Product.IsFavorite = Product.IsFavorite;

                    await LastCheckedProductsCache.SaveLastCheckedProducts(Product);
                }

                if(Product.Sizes != null)
                {
                    Sizes = [.. Product.Sizes];
                }

                if (Product?.Variants != null && Product.Variants.Count > 0)
                {
                    foreach (var variant in Product.Variants)
                    {
                        variant.IsSelected = variant.Id == ProductId;
                    }
                }

                var reviewSearch = new PagedSearchId
                {
                    Id = ProductId,
                    Page = 1,
                    PerPage = 6,
                };
                var reviews = await _reviewService.GetReviewsAsync(reviewSearch);

                if (reviews != null && reviews.Data != null)
                {
                    Reviews = [.. reviews.Data];
                    IsReviewsVisible = reviews.TotalCount > 0;
                }

                UpdateImageInidactor();
                await LoadRecent();
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
        private void SelectSize(SizeModel selectedSize)
        {
            foreach (var size in Sizes)
                size.IsSelected = false;

            selectedSize.IsSelected = true;
            SelectedSize = selectedSize;
        }

        private async Task LoadRecent()
        {
            var recent = await LastCheckedProductsCache.GetLastCheckedProducts();

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

        [RelayCommand]
        private async Task ChangeVaraint(int id)
        {
            try
            {
                ProductId = id;
                await LoadProduct();

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task GoToReviewsPage()
        {
            await Shell.Current.GoToAsync($"{nameof(ReviewsPage)}?ProductId={ProductId}");
        }

        [RelayCommand]
        private async Task AddOrRemoveFavorite()
        {
            try
            {
                if (Product == null) return;

                if (Product.IsFavorite)
                {
                    var result = await _favoriteService.RemoveFromFavoritesAsync(Product.Id);

                    if (result)
                    {
                        Product.IsFavorite = false;
                    }
                }
                else
                {
                    var result = await _favoriteService.AddToFavoritesAsync(Product.Id);

                    if (result)
                    {
                        Product.IsFavorite = true;
                    }
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        public async Task BuyNow()
        {
            try
            {
                if (SelectedSize == null)
                {
                    var popup = new MessagePopup("Greška", "Molimo vas izaberite velicinu pre nego što dodate proizvod u korpu.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
                else
                {
                    await _cartService.AddItem(new CartModel
                    {
                        Product = Product,
                        Size = SelectedSize,
                        Quantity = 1,
                    });

                    await Shell.Current.GoToAsync($"//{nameof(CartPage)}");
                }
            }
            catch (UserNotFoundException)
            {
                var popup = new MessagePopup("Greška", "Niste ulogovani. Molimo da se ulogujete da bi ste dodali proizvod u korpu.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        public async Task AddToCart()
        {
            try
            {
                if (SelectedSize == null)
                {
                    var popup = new MessagePopup("Greška", "Molimo vas izaberite velicinu pre nego što dodate proizvod u korpu.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
                else
                {
                    await _cartService.AddItem(new CartModel
                    {
                        Product = Product,
                        Size = SelectedSize,
                        Quantity = 1,
                    });

                    var popup = new MessagePopup("", "Uspešno ste dodali proizvod u korpu.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
            }
            catch (UserNotFoundException)
            {
                var popup = new MessagePopup("Greška", "Niste ulogovani. Molimo da se ulogujete da bi ste dodali proizvod u korpu.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }
    }
}

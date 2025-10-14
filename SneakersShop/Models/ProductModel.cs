using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SneakersShop.Helpers;

namespace SneakersShop.Models
{
    public class ProductsModel
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal OldPrice { get; set; }
        public int BrandId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public decimal? NewPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public int SoldQuantity { get; set; }

        public string Thumbnail => $"{AppConstants.IMAGE_URL}{Image}";

        public bool HasDiscount => DiscountValue != null && DiscountValue != 0;

        public Color TextColor =>
            HasDiscount
                ? Microsoft.Maui.Graphics.Color.FromArgb("#ACACAC")
                : Microsoft.Maui.Graphics.Color.FromArgb("#FF5500");
    }

    public partial class ProductModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public List<ProductVariantModel>? Variants { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public string Code { get; set; } = string.Empty;
        public double OldPrice { get; set; }
        public double? NewPrice { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal? DiscountValue { get; set; }
        [ObservableProperty]
        private bool isFavorite = false;
        public List<string>? Images { get; set; }
        public List<SizeModel>? Sizes { get; set; }

        public string FavoriteImage => IsFavorite ? "heart_selected.svg" : "heart.svg";

        public List<string> FullImageUrls
        {
            get
            {
                if (Images == null)
                    return [];

                return
                [
                    .. Images
                        .OrderByDescending(x => x)
                        .Select(img => $"{AppConstants.IMAGE_URL}{img}"),
                ];
            }
        }
        public bool HasDiscount => DiscountValue != null && DiscountValue != 0;

        public Color TextColor =>
            HasDiscount
                ? Microsoft.Maui.Graphics.Color.FromArgb("#ACACAC")
                : Microsoft.Maui.Graphics.Color.FromArgb("#FF5500");

        partial void OnIsFavoriteChanged(bool value)
        {
            OnPropertyChanged(nameof(FavoriteImage));
        }
    }

    public class ProductVariantModel : BaseModel
    {
        public string Image { get; set; } = string.Empty;
        public bool IsSelected { get; set; }

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
        public Color BorderColor => IsSelected ? Color.FromArgb("#FF5500") : Colors.Transparent;
    }
}

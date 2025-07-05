using SneakersShop.Helpers;
using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
    public abstract class OrangeColor : BaseModel
    {
        public Color OrangeTextColor => Color.FromArgb("#FF5500");
    }

    public class ProductsModel : OrangeColor
    {        
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public string? Code { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public double OldPrice { get; set; }
        public double? NewPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public string? Image { get; set; }

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";

        [JsonIgnore]
        public bool HasDiscount => DiscountValue != null && DiscountValue != 0;

        [JsonIgnore]
        public bool HasDiscountType => DiscountType != null;

        [JsonIgnore]
        public bool HasDiscountValue => DiscountValue > 0;

        [JsonIgnore]
        public Color TextColor => HasDiscount ? Colors.Gray : OrangeTextColor;

    }

    public class ProductModel : OrangeColor
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public List<ProductVariantModel>? Variants { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public string? Code { get; set; }
        public double OldPrice { get; set; }
        public double? NewPrice { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public bool IsFavorite { get; set; }
        public List<string>? Images { get; set; }
        public List<SizeModel>? Sizes { get; set; }

        [JsonIgnore]
        public bool IsNewPriceNotNull => NewPrice != null;

        [JsonIgnore]
        public List<string> FullImageUrls
        {
            get
            {
                if (Images == null)
                    return new List<string>();

                return [.. Images.OrderByDescending(x => x).Select(img => $"{AppConstants.IMAGE_URL}{img}")];
            }
        }

        [JsonIgnore]
        public bool HasDiscount => DiscountValue != null && DiscountValue != 0;

        [JsonIgnore]
        public bool HasDiscountType => DiscountType != null;

        [JsonIgnore]
        public bool HasDiscountValue => DiscountValue > 0;

        [JsonIgnore]
        public Color TextColor => HasDiscount ? Colors.Gray : OrangeTextColor;
    }

    public class ProductVariantModel : BaseModel
    {
        public string? Image { get; set; }
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
        [JsonIgnore]
        public Color BorderColor => IsSelected ? Color.FromArgb("#FF5500") : Colors.Transparent;
    }
}

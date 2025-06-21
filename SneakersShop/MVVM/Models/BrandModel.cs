using SneakersShop.Helpers;
using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
    public class BrandModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }
}

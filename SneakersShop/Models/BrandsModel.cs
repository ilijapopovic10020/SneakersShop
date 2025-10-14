using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using SneakersShop.Helpers;

namespace SneakersShop.Models
{
    public partial class BrandsModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        [ObservableProperty]
        private bool isSelected = false;
        

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }
}

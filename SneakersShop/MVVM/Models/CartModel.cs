using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
    public partial class CartModel : ObservableObject
    {
        public SizeModel Size { get; set; }

        [ObservableProperty]
        private ProductModel product;

        [ObservableProperty]
        private int quantity;

        [JsonIgnore]
        [ObservableProperty]
        private bool isSelected = true;
    }
}

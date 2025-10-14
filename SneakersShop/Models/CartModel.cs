using CommunityToolkit.Mvvm.ComponentModel;

namespace SneakersShop.Models
{
    public partial class CartModel : ObservableObject
    {
        public ProductModel Product { get; set; } = new();

        public SizeModel Size { get; set; } = new();

        [ObservableProperty]
        private int quantity;

        [ObservableProperty]
        private bool isSelected = true;
    }
}

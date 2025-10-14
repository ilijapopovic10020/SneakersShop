using CommunityToolkit.Mvvm.ComponentModel;

namespace SneakersShop.Models
{
    public partial class SizeModel : ObservableObject
    {
        public int Id { get; set; }
        public decimal Number { get; set; }
        public string Detail { get; set; } = string.Empty;
        public int Quantity { get; set; }

        [ObservableProperty]
        private bool isSelected;

        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }
}

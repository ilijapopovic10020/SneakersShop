using CommunityToolkit.Mvvm.ComponentModel;
using SneakersShop.Extensions;
using SneakersShop.Models;

namespace SneakersShop.Helpers
{
    public partial class OrderStatusOption : ObservableObject
    {
        [ObservableProperty]
        public OrderStatus status;
        public string DisplayName => Status.GetDescription();

        [ObservableProperty]
        public bool isSelected;
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Colors.Transparent;
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;

        partial void OnIsSelectedChanged(bool value)
        {
            OnPropertyChanged(nameof(BackgroundColor));
            OnPropertyChanged(nameof(TextColor));
        }
    }
}

using SneakersShop.Extensions;
using SneakersShop.Models;

namespace SneakersShop.Helpers
{
    public class FilterOption
    {
        public FilterModel Filter { get; set; }
        public string DisplayName => Filter.GetDescription();

        public bool IsSelected { get; set; }
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }
}

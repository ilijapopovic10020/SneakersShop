using SneakersShop.Helpers.Extensions;
using System.Text.Json.Serialization;

namespace SneakersShop.Helpers
{
    public class FilterOption
    {
        public Filter Filter { get; set; }
        public string DisplayName => Filter.GetDescription();

        [JsonIgnore]
        public bool IsSelected { get; set; }
        [JsonIgnore]
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        [JsonIgnore]
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }
}

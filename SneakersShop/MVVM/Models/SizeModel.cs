using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
    public class SizeModel
    {
        public int Id { get; set; }
        public decimal Number { get; set; }
        public string? Detail { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }
        [JsonIgnore]
        public Color BackgroundColor => Quantity == 0 ? 
            Color.FromArgb("#e1e1e1") : IsSelected ? 
            Color.FromArgb("#FF5500") : Color.FromArgb("#000000");
        [JsonIgnore]
        public Color TextColor => Quantity == 0 ? 
            Color.FromArgb("#212121") : Color.FromArgb("#ffffff");
    }
}

namespace SneakersShop.Models
{
    public class CategoriesModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }
}

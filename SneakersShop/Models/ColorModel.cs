namespace SneakersShop.Models
{
    public class ColorsModel : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }
}

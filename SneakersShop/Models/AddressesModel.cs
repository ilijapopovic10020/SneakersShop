namespace SneakersShop.Models
{

    public class AddressesModel : BaseModel
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public bool IsDefault { get; set; }

        public bool IsSelected { get; set; }
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }

    public class CreateAddressModel
    {
        public int CityId { get; set; }
        public string Street { get; set; } = string.Empty;
    }

    public class UpdateAddressModel
    {
        public int CityId { get; set; }
        public string Street { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}

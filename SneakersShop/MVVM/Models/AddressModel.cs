using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
    public class AddressModel : BaseModel
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }
        [JsonIgnore]
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#FF5500") : Color.FromArgb("#F6F6F6");
        [JsonIgnore]
        public Color TextColor => IsSelected ? Colors.White : Colors.Black;
    }

    public class CreateAddressModel
    {
        public int CityId { get; set; }
        public string Street { get; set; }
    }

    public class UpdateAddressModel
    {
        public int CityId { get; set; }
        public string Street { get; set; }
        public bool IsDefault { get; set; }
    }
}

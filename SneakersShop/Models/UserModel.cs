using SneakersShop.Helpers;

namespace SneakersShop.Models
{
    public class RegisterUserModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Phone { get; set; } = string.Empty;
    }

    public class UserModel : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Image { get; set; }
        public string? Phone { get; set; }
        public IEnumerable<AddressesModel> Addresses { get; set; } = [];

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }

    public class UpdateUserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Phone { get; set; }
    }
}

using SneakersShop.Helpers;

namespace SneakersShop.MVVM.Models
{
    public class LoginModel : BaseModel
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime LoginExparation { get; set; }
        public bool ShouldBeLoggedOut => LoginExparation < DateTime.UtcNow;
    }

    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string Phone { get; set; }
    }

    public class UserModel : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Image { get; set; }
        public string? Phone { get; set; }
        public IEnumerable<AddressModel> Addresses { get; set; }

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }

    public class UserUpdateModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Phone { get; set; }
    }
}

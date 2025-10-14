using SneakersShop.Helpers;

namespace SneakersShop.Models
{
    public class ReviewsModel : BaseModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserImage { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{UserImage}";
    }

    public class CreateReviewModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}

using SneakersShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
namespace SneakersShop.MVVM.Models
{
    public class ReviewsModel : BaseModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserImage { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string CreatedAt { get; set; }

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{UserImage}";
    }

    public class CreateReviewModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}

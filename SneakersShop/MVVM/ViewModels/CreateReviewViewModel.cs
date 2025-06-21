using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class CreateReviewViewModel : ObservableObject
    {
        private readonly ReviewService _reviewService;

        [ObservableProperty]
        private CreateReviewModel review;

        public CreateReviewViewModel()
        {
            _reviewService = new();
        }

        [RelayCommand]
        private async Task CreateReview(int id)
        {

        }
    }
}

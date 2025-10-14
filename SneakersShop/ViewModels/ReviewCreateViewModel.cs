using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Extensions;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(ProductId), "ProductId")]
    public partial class ReviewCreateViewModel : ObservableObject
    {
        private readonly IReviewService _reviewService;

        public ReviewCreateViewModel(IReviewService reviewService)
        {
            _reviewService = reviewService;

            Stars = [];
        }

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private ObservableCollection<StarsModel> stars;

        [ObservableProperty]
        private string comment = string.Empty;

        [ObservableProperty]
        private int selectedRating;


        [RelayCommand]
        private void LoadStars()
        {
            Stars = 
            [
                new() {Index = 1, IsFilled = false},
                new() {Index = 2, IsFilled = false},
                new() {Index = 3, IsFilled = false},
                new() {Index = 4, IsFilled = false},
                new() {Index = 5, IsFilled = false}
            ];
        }

        [RelayCommand]
        private void SetRating(int index)
        {
            SelectedRating = index;

            foreach (var star in Stars)
                star.IsFilled = star.Index <= index;
        }

        [RelayCommand]
        private async Task CreateReview()
        {
            try
            {
                var user = await SecureStorage.Default.GetUser();

                if (string.IsNullOrEmpty(Comment) || SelectedRating == 0)
                {
                    var popup = new MessagePopup("Greška", "Molimo unesite komentar i ocenu.");
                    await Shell.Current.ShowPopupAsync(popup);
                    return;
                }

                var reviewModel = new CreateReviewModel()
                {
                    ProductId = ProductId,
                    UserId = user.Id,
                    Comment = Comment,
                    Rating = SelectedRating
                };

                var result = await _reviewService.CreateReviewAsync(reviewModel);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Uspešno ste kreirali recenziju.");
                    await Shell.Current.ShowPopupAsync(popup);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }
    }
}

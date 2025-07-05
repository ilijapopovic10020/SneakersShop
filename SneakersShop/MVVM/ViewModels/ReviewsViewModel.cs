using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class ReviewsViewModel : ObservableObject
    {
        private readonly ReviewService _reviewService;

        [ObservableProperty]
        private ObservableCollection<ReviewsModel> reviews;

        [ObservableProperty]
        private ObservableCollection<StarsModel> stars;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private double avgReview;

        [ObservableProperty]
        private bool isLoading = true;

        [ObservableProperty]
        private Dictionary<int, int> ratingCounts = new() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

        [ObservableProperty]
        private Dictionary<int, double> ratingWidth = new() { { 1, 0.0 }, { 2, 0.0 }, { 3, 0.0 }, { 4, 0.0 }, { 5, 0.0 } };

        [ObservableProperty]
        private int selectedRating;

        public ReviewsViewModel()
        {
            _reviewService = new();

            Reviews = [];
        }

        public int MaxRatingCount => RatingCounts?.Values.Max() ?? 1;

        private double GetRatingBarWidth(int rating)
        {
            double maxWidth = 150;

            if (RatingCounts == null || !RatingCounts.ContainsKey(rating) || MaxRatingCount == 0)
                return 0;

            return (RatingCounts[rating] / (double)MaxRatingCount * maxWidth);
        }

        [RelayCommand]
        private void LoadStars()
        {
            Stars =
            [
                new() { Index = 1, IsFilled = false },
                new() { Index = 2, IsFilled = false },
                new() { Index = 3, IsFilled = false },
                new() { Index = 4, IsFilled = false },
                new() { Index = 5, IsFilled = false }
            ];
        }

        [RelayCommand]
        private async Task LoadReviews(int id)
        {
            try
            {
                IsLoading = true;

                var search = new PagedSearchId
                {
                    Id = id,
                    Page = 1,
                    PerPage = 100
                };

                var reviews = await _reviewService.Get(search);

                if (reviews != null && reviews.Data != null)
                {
                    Reviews = [.. reviews.Data.Take(10)];
                    TotalCount = reviews.TotalCount;
                    AvgReview = Math.Round(Reviews.Average(r => r.Rating), 2);

                    var counts = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

                    foreach (var review in reviews.Data)
                    {
                        if (counts.TryGetValue(review.Rating, out int value))
                            counts[review.Rating] = ++value;
                    }

                    RatingCounts = counts;
                    OnPropertyChanged(nameof(MaxRatingCount));

                    RatingWidth = counts.ToDictionary(
                        kvp => kvp.Key,
                        kvp => GetRatingBarWidth(kvp.Key)
                    );
                }

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                if (App.Current != null && App.Current.MainPage != null)
                    await App.Current.MainPage.ShowPopupAsync(popup);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void SetRating(int index)
        {
            SelectedRating = index;
            for (int i = 0; i< Stars.Count; i++)
            {
                if (i < index)
                    Stars[i].IsFilled = i < index;
            }
        }       
    }
}

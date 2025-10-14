using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(ProductId), "ProductId")]
    public partial class ReviewsViewModel : ObservableObject
    {
        private readonly IReviewService _reviewService;

        public ReviewsViewModel(IReviewService reviewService)
        {
            _reviewService = reviewService;

            Reviews = [];
        }

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private ObservableCollection<ReviewsModel> reviews;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int itemsPerPage = 10;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private bool isPageinationAvailable = true;

        [ObservableProperty]
        private bool isNextPageAvailable = true;

        [ObservableProperty]
        private bool isPreviousPageAvailable = true;

        [ObservableProperty]
        private Dictionary<int, int> ratingCounts = new() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

        [ObservableProperty]
        private Dictionary<int, double> ratingWidth = new() { { 1, 0.0 }, { 2, 0.0 }, { 3, 0.0 }, { 4, 0.0 }, { 5, 0.0 } };

        [ObservableProperty]
        private double avgReview = 0.0;

        [RelayCommand]
        private async Task LoadReviews()
        {
            try
            {
                IsLoading = true;

                var reviews = await _reviewService.GetReviewsAsync(new Models.Search.PagedSearchId
                {
                    Id = ProductId,
                    Page = CurrentPage,
                    PerPage = ItemsPerPage
                });


                if (reviews != null && reviews.Data != null)
                {
                    Reviews = [.. reviews.Data];

                    TotalCount = reviews.TotalCount;
                    PagesCount = reviews.PagesCount;

                    if (PagesCount == 1)
                        IsPageinationAvailable = false;
                    else
                        IsPageinationAvailable = true;

                    if (CurrentPage == 1)
                        IsPreviousPageAvailable = false;
                    else
                        IsPreviousPageAvailable = true;

                    if (CurrentPage == PagesCount)
                        IsNextPageAvailable = false;
                    else
                        IsNextPageAvailable = true;


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

        public int MaxRatingCount => RatingCounts?.Values.Max() ?? 1;

        private double GetRatingBarWidth(int rating)
        {
            double maxWidth = 100;

            if (RatingCounts == null || !RatingCounts.ContainsKey(rating) || MaxRatingCount == 0)
                return 0;

            return (RatingCounts[rating] / (double)MaxRatingCount * maxWidth);
        }

        [RelayCommand]
        private async Task GoToReviewCreatePage()
        {
            await Shell.Current.GoToAsync($"{nameof(ReviewCreatePage)}?ProductId={ProductId}");
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (CurrentPage < PagesCount)
            {
                CurrentPage++;
                await LoadReviews();
            }
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadReviews();
            }
        }
    }
}

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Models.Search;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    public partial class FiltersViewModel : ObservableObject
    {
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;

        [ObservableProperty]
        private ObservableCollection<BrandsModel> brands;

        [ObservableProperty]
        private List<int>? selectedBrands;

        [ObservableProperty]
        private ObservableCollection<ColorsModel> colors;

        [ObservableProperty]
        private bool isShowingAllColors = false;

        [ObservableProperty]
        private ObservableCollection<ColorsModel> visibleColors;

        [ObservableProperty]
        private List<string>? selectedColors;

        [ObservableProperty]
        private decimal? minPrice;

        [ObservableProperty]
        private decimal? maxPrice;

        [ObservableProperty]
        private bool isLoading = false;

        public FiltersViewModel(IBrandService brandService, IColorService colorService)
        {
            _brandService = brandService;
            _colorService = colorService;

            Brands = [];
            SelectedBrands = [];

            Colors = [];
            SelectedColors = [];
            VisibleColors = [];
        }

        [RelayCommand]
        private async Task LoadFilters()
        {
            try
            {
                IsLoading = true;

                var brands = await _brandService.GetBrandsAsync();
                Brands = [.. brands];

                var colors = await _colorService.GetColorsAsync();

                if (colors != null)
                {
                    Colors = [.. colors.Where(c => !c.Name.Contains('-'))];
                    UpdateVisibleColors();
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void ToggleColorVisibility()
        {
            IsShowingAllColors = !IsShowingAllColors;
            UpdateVisibleColors();
        }

        private void UpdateVisibleColors()
        {
            if (IsShowingAllColors)
            {
                VisibleColors = [.. Colors];
            }
            else
            {
                VisibleColors = [.. Colors.Take(5)];
            }
        }

        [RelayCommand]
        private async Task AppyFilters()
        {
            if (MaxPrice <= 0)
                MaxPrice = null;

            SelectedColors = [.. Colors.Where(color => color.IsSelected).Select(color => color.Name)];

            SelectedBrands = [.. Brands.Where(brand => brand.IsSelected).Select(brand => brand.Id)];

            var brandsString = string.Join(",", SelectedBrands);

            var colorsString = string.Join(",", SelectedColors);

            await Shell.Current.GoToAsync($"//{nameof(ProductsPage)}?SelectedBrands={brandsString}&SelectedColors={colorsString}&MinPrice={MinPrice}&MaxPrice={MaxPrice}");
        }
    }
}

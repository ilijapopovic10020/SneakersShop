using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    public partial class FiltersViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;

        public FiltersViewModel(IBrandService brandService, IColorService colorService)
        {
            _brandService = brandService;
            _colorService = colorService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedBrands", out var brands))
                SelectedBrands = brands as List<int> ?? [];

            if (query.TryGetValue("SelectedColors", out var colors))
                SelectedColors = colors as List<string> ?? [];

            if (query.TryGetValue("MinPrice", out var minPrice))
                MinPrice = (decimal?)(minPrice ?? null);

            if (query.TryGetValue("MaxPrice", out var maxPrice))
                MaxPrice = (decimal?)(maxPrice ?? null);
        }

        [ObservableProperty]
        private ObservableCollection<BrandsModel> brands = [];

        [ObservableProperty]
        private List<int>? selectedBrands = [];

        [ObservableProperty]
        private ObservableCollection<ColorsModel> colors = [];

        [ObservableProperty]
        private List<string>? selectedColors = [];

        [ObservableProperty]
        private decimal? minPrice;

        [ObservableProperty]
        private decimal? maxPrice;



        [RelayCommand]
        private async Task LoadFilters()
        {
            try
            {
                var allBrands = await _brandService.GetBrandsAsync();
                Brands = [.. allBrands];

                var allColors = await _colorService.GetColorsAsync();
                Colors = [.. allColors];

                foreach (var brand in Brands)
                    brand.IsSelected = SelectedBrands.Contains(brand.Id);

                foreach (var color in Colors)
                    color.IsSelected = SelectedColors.Contains(color.Name);
            }
            catch (TaskCanceledException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (HttpRequestException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task ApplyFilters()
        {
            await Shell.Current.GoToAsync($"//{nameof(ProductsPage)}", true, new Dictionary<string, object?>
            {
                { "SelectedBrands", SelectedBrands },
                { "SelectedColors", SelectedColors },
                { "MinPrice", MinPrice },
                { "MaxPrice", MaxPrice }
            });
        }
    }
}

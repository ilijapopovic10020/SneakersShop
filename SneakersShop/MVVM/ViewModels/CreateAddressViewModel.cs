using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class CreateAddressViewModel : ObservableObject
    {
        private readonly CityService _cityService;
        private readonly AddressService _addressService;

        [ObservableProperty]
        private ObservableCollection<CityModel> cities;

        [ObservableProperty]
        private CityModel selectedCity;

        [ObservableProperty]
        private string street;

        public CreateAddressViewModel()
        {
            _cityService = new CityService();
            _addressService = new AddressService();

            Cities = [];
        }

        [RelayCommand]
        private async Task LoadCities()
        {
            var cities = await _cityService.Get();

            if (cities != null)
            {
                Cities = [.. cities];
            }
        }

        [RelayCommand]
        private async Task AddNewAddress()
        {
            try
            {
                if (Street == null)
                {
                    var popup = new MessagePopup("Greška", "Ulica i broj je obavezno polje.");
                    await App.Current.MainPage.ShowPopupAsync(popup);
                    return;
                }
                if(SelectedCity == null)
                {
                    var popup = new MessagePopup("Greška", "Grad je obavezno polje.");
                    await App.Current.MainPage.ShowPopupAsync(popup);
                    return;
                }

                var address = new CreateAddressModel
                {
                    Street = Street,
                    CityId = SelectedCity.Id
                };                

                var result = await _addressService.Post(address);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Adresa je uspešno kreirana.");
                    await App.Current.MainPage.ShowPopupAsync(popup);
                    await Shell.Current.GoToAsync("..");
                }

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}

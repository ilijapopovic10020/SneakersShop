using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class UpdateAddressViewModel : ObservableObject
    {
        private readonly AddressService _addressService;
        private readonly CityService _cityService;

        [ObservableProperty]
        private AddressModel address;

        [ObservableProperty]
        private ObservableCollection<CityModel> cities;

        [ObservableProperty]
        private CityModel selectedCity;

        public UpdateAddressViewModel()
        {
            _addressService = new();
            _cityService = new();

            cities = [];
        }

        [RelayCommand]
        private async Task LoadCities()
        {
            var cities = await _cityService.Get();

            if (cities != null)
            {
                Cities = [.. cities];

                if (Address != null)
                {
                    SelectedCity = Cities.FirstOrDefault(c => c.Name == Address.City);
                }
            }
           
        }

        [RelayCommand]
        private async Task UpdateAddress()
        {
            try
            {
                if (!Address.IsDefault)
                {
                    var existingAddresses = await _addressService.Get();

                    bool someOtherDefault = existingAddresses
                        .Any(a => a.IsDefault && a.Id != Address.Id);

                    if (!someOtherDefault)
                    {
                        var popup = new MessagePopup("Greška", "Mora postojati bar jedna podrazumevana adresa.");
                        await App.Current.MainPage.ShowPopupAsync(popup);
                        return;
                    }
                }

                var updateModel = new UpdateAddressModel
                {
                    Street = Address.Street,
                    CityId = SelectedCity.Id,
                    IsDefault = Address.IsDefault
                };

                var success = await _addressService.Update(Address.Id, updateModel);

                if (success)
                {
                    var popup = new MessagePopup("Uspeh", "Adresa je uspešno izmenjena.");
                    await App.Current.MainPage.ShowPopupAsync(popup);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    var popup = new MessagePopup("Greška", "Došlo je do greške prilikom izmene adrese.");
                    await App.Current.MainPage.ShowPopupAsync(popup);
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

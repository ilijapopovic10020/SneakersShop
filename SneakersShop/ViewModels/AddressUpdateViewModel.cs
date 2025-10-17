using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class AddressUpdateViewModel : ObservableObject
    {
        private readonly IAddressService _addressService;
        private readonly ICityService _cityService;

        public AddressUpdateViewModel(IAddressService addressService, ICityService cityService)
        {
            _addressService = addressService;
            _cityService = cityService;

            Address = new();
            SelectedCity = new();
            Cities = [];
        }

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private AddressesModel address;

        [ObservableProperty]
        private ObservableCollection<CityModel> cities;

        [ObservableProperty]
        private CityModel selectedCity;

        [ObservableProperty]
        private bool isLoading = false;

        [RelayCommand]
        private async Task LoadData()
        {
            try
            {
                IsLoading = true;

                Address = await _addressService.GetAddressByIdAsync(Id);

                var cities = await _cityService.GetCitiesAsync();
                Cities = [.. cities];

                if(Address != null)
                {
                    SelectedCity = Cities.FirstOrDefault(c => c.Name == Address.City);
                }
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
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateAddress()
        {
            try
            {
                if (!Address.IsDefault)
                {
                    var existingAddresses = await _addressService.GetAddressesAsync();

                    bool someOtherDefault = existingAddresses
                        .Any(a => a.IsDefault && a.Id == Id);

                    if (!someOtherDefault)
                    {
                        var popup = new MessagePopup("Greška", "Mora postojati bar jedna podrazumevana adresa. Molimo pokušajte ponovo.");
                        await Shell.Current.ShowPopupAsync(popup);
                        return;
                    }
                }

                var updateModel = new UpdateAddressModel()
                {
                    Street = Address.Street,
                    CityId = SelectedCity.Id,
                    IsDefault = Address.IsDefault
                };

                var result = await _addressService.UpdateAddressAsync(Address.Id, updateModel);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Adresa je uspešno izmenjena.");
                    await Shell.Current.ShowPopupAsync(popup);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    var popup = new MessagePopup("Greška", "Došlo je do greške prilikom izmene adrese. Molimo pokušajte ponovo.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
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
    }
}

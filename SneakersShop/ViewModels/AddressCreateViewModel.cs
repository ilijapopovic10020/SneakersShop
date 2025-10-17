using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Net;

namespace SneakersShop.ViewModels
{
    public partial class AddressCreateViewModel : ObservableObject
    {
        private readonly IAddressService _addressService;
        private readonly ICityService _cityService;

        public AddressCreateViewModel(IAddressService addressService, ICityService cityService)
        {
            _addressService = addressService;
            _cityService = cityService;

            SelectedCity = new();
            Cities = [];
        }

        [ObservableProperty]
        private ObservableCollection<CityModel> cities;

        [ObservableProperty]
        private CityModel selectedCity;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string street = string.Empty;

        [RelayCommand]
        private async Task LoadCities()
        {
            try
            {
                IsLoading = true;

                var cities = await _cityService.GetCitiesAsync();
                Cities = [.. cities];
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
        private async Task CreateAddress()
        {
            try
            {
                if(string.IsNullOrEmpty(Street) || SelectedCity == null)
                {
                    var popup = new MessagePopup("Greška", "Molimo unesite naziv ulice i broj i izaberite grad.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
                else
                {
                    var addressModel = new CreateAddressModel
                    {
                        CityId = SelectedCity.Id,
                        Street = Street
                    };

                    var result = await _addressService.CreateAddressAsync(addressModel);

                    if (result)
                    {
                        var popup = new MessagePopup("Uspeh", "Adresa je uspešno kreirana.");
                        await Shell.Current.ShowPopupAsync(popup);
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        var popup = new MessagePopup("Greška", "Došlo je do greške prilikom kreiranja adrese. Molimo pokušajte ponovo.");
                        await Shell.Current.ShowPopupAsync(popup);
                    }
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

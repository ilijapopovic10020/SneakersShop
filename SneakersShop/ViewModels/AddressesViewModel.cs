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
    public partial class AddressesViewModel : ObservableObject
    {
        private readonly IAddressService _addressService;

        public AddressesViewModel(IAddressService addressService)
        {
            _addressService = addressService;

            Addresses = [];
        }

        [ObservableProperty]
        private ObservableCollection<AddressesModel> addresses;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isAddNewAddressVisible = true;

        [RelayCommand]
        private async Task LoadAddresses()
        {
            try
            {
                IsLoading = true;

                var addresses = await _addressService.GetAddressesAsync();

                Addresses = [.. addresses];


                IsAddNewAddressVisible = addresses.Count() < 3;
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
        private async Task DeleteAddress(int id)
        {
            try
            {
                var deleted = await _addressService.DeleteAddressAsync(id);

                if (deleted)
                {
                    var popup = new MessagePopup("Poruka", "Uspešno ste obrisali adresu.");
                    await Shell.Current.ShowPopupAsync(popup);
                }

                await LoadAddresses();
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        [RelayCommand]
        private async Task EditAddress(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(AddressUpdatePage)}?Id={id}");
        }
    }
}

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class AddressesViewModel : ObservableObject
    {
        private readonly AddressService _addressService;

        [ObservableProperty]
        private ObservableCollection<AddressModel> addresses;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool isAddNewAddressVisible = true;

        public AddressesViewModel()
        {
            _addressService = new();

            Addresses = [];
        }

        [RelayCommand]
        private async Task LoadAddresses()
        {
            try
            {
                IsLoading = true;

                var addresses = await _addressService.Get();
                if (addresses != null)
                {
                    Addresses = [.. addresses];
                    IsAddNewAddressVisible = addresses.Count() < 3;
                }

            }
            catch (Exception ex)
            {

                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
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
                var deleted =  await _addressService.Delete(id);

                if (deleted)
                {
                    var popup = new MessagePopup("Poruka", "Uspešno ste obrisali adresu.");
                    var result = await App.Current.MainPage.ShowPopupAsync(popup);
                }

                await LoadAddresses();
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}

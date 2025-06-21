using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using SneakersShop.Components;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CartModel> cartItems;

        [ObservableProperty]
        private bool isAllSelected = true;

        private bool isSyncingSelection = false;

        public bool IsCartVisible => CartItems.Count > 0;
        public double? TotalPrice => CartItems.Where(x => x.IsSelected).Sum(x => x.Product.NewPrice != null ? x.Product.NewPrice * x.Quantity : x.Product.OldPrice * x.Quantity);
        public int CartQuantitySelectedItems => CartItems.Where(x => x.IsSelected).Sum(x => x.Quantity);
        public int CartQuantity => CartItems.Sum(x => x.Quantity);
        public string CartQuantityText => $"Vaša korpa ({CartQuantity})";
        public bool IsCheckoutButtonVisible => TotalPrice > 0;
        public string CheckoutButtonText => $"Završi kopuvinu {TotalPrice:N2} RSD";

        public CartViewModel()
        {
            CartItems = [];
        }

        [RelayCommand]
        private void LoadCartItems()
        {
            CartItems = [.. CartService.Instance.GetItems()];

            foreach (var item in CartItems)
            {
                item.PropertyChanged += CartItem_PropertyChanged;
            }

            RefreshCartUI();
        }

        partial void OnIsAllSelectedChanged(bool value)
        {
            if (isSyncingSelection) return;

            isSyncingSelection = true;

            foreach (var item in CartItems)
            {
                item.IsSelected = value;
            }

            isSyncingSelection = false;
        }
        
        private void CartItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartModel.IsSelected))
            {
                RefreshCartUI();

                if (isSyncingSelection) return;

                isSyncingSelection = true;

                IsAllSelected = CartItems.All(x => x.IsSelected);

                isSyncingSelection = false;
            }
        }

        [RelayCommand]
        private async Task SelectSize(CartModel item)
        {
            var popup = new SizePopup(item.Product.Sizes, item.Size);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is SizeModel selected)
            {
                item.Size = selected;
                item.Size.IsSelected = true;

                await CartService.Instance.Upsert(CartItems.ToList());
                LoadCartItems();
            }
        }

        [RelayCommand]
        private async Task IncreaseQuantity(int id)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == id);

            if (cartItem == null)
            {
                await SnackbarHelper.ShowMessage("Došlo je do greške, molimo pokušajte ponovo.");
                return;
            }

            cartItem.Quantity++;

            RefreshCartUI();
            await CartService.Instance.Upsert(CartItems.ToList());
        }

        [RelayCommand]
        private async Task DecreaseQuantity(int id)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == id);

            if (cartItem == null)
            {
                await SnackbarHelper.ShowMessage("Došlo je do greške, molimo pokušajte ponovo.");
                return;
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                RefreshCartUI();
                await CartService.Instance.Upsert(CartItems.ToList());
            }
            else
            {
                await RemoveCartItem(id);
            }

        }

        [RelayCommand]
        private async Task RemoveCartItem(int id)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == id);

            if (cartItem == null)
            {
                await SnackbarHelper.ShowMessage("Došlo je do greške, molimo pokušajte ponovo.");
                return;
            }

            CartItems.Remove(cartItem);
            await CartService.Instance.RemoveItem(cartItem);
            RefreshCartUI();

        }

        [RelayCommand]
        private async Task ClearCartItems()
        {
            CartItems.Clear();
            await CartService.Instance.ClearCart();
            RefreshCartUI();
        }

        public async Task RemovePurchasedItems()
        {
            var selectedItems = CartItems.Where(x => x.IsSelected).ToList();

            foreach (var item in selectedItems)
            {
                CartItems.Remove(item);
            }

            await CartService.Instance.RemoveItems(selectedItems);
            RefreshCartUI();
        }

        private void RefreshCartUI()
        {
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(CheckoutButtonText));
            OnPropertyChanged(nameof(CartQuantity));
            OnPropertyChanged(nameof(CartQuantityText));
            OnPropertyChanged(nameof(IsCartVisible));
            OnPropertyChanged(nameof(IsCheckoutButtonVisible));
        }
    }
}

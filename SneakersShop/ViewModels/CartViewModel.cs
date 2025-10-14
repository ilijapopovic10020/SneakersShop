using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SneakersShop.Components.Popups;
using SneakersShop.Views;
using SneakersShop.Extensions;
using SneakersShop.Exceptions;

namespace SneakersShop.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        private readonly ICartService _cartService;

        [ObservableProperty]
        private ObservableCollection<CartModel> cartItems = [];

        [ObservableProperty]
        private bool isAllSelected = true;

        private bool isSyncingSelection = false;

        public CartViewModel(ICartService cartService)
        {
            _cartService = cartService;
            CartItems = _cartService.CartItems;

            foreach (var item in CartItems)
                item.PropertyChanged += CartItem_PropertyChanged;
        }

        public bool IsCartVisible => CartItems.Count > 0;

        public double? TotalPrice =>
            CartItems.Where(x => x.IsSelected)
                     .Sum(x => (x.Product.NewPrice ?? x.Product.OldPrice) * x.Quantity);

        public int CartQuantitySelectedItems =>
            CartItems.Where(x => x.IsSelected).Sum(x => x.Quantity);

        public int CartQuantity => CartItems.Sum(x => x.Quantity);

        public string CartQuantityText => $"Vaša korpa ({CartQuantity})";

        public bool IsCheckoutButtonVisible => TotalPrice > 0;

        public string CheckoutButtonText => $"Završi kupovinu {TotalPrice:N2} RSD";

        [RelayCommand]
        private async Task LoadCartItemsAsync()
        {
            try
            {
                try
                {
                    var serverItems = await _cartService.GetCartItemsAsync();
                    _cartService.LoadFromServer(serverItems);

                    foreach (var item in CartItems)
                        item.PropertyChanged += CartItem_PropertyChanged;

                    RefreshCartUI();
                }
                catch (Exception ex)
                {
                    if (ex is UserNotFoundException)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        var popup = new MessagePopup("Greška", ex.Message);
                        await Shell.Current.ShowPopupAsync(popup);
                    }
                }
                
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        partial void OnIsAllSelectedChanged(bool value)
        {
            if (isSyncingSelection) return;

            isSyncingSelection = true;

            foreach (var item in CartItems)
                item.IsSelected = value;

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
        private async Task SelectSizeAsync(CartModel item)
        {
            var popup = new SizePopup(item.Product.Sizes, item.Size);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is SizeModel selected)
            {
                item.Size = selected;
                item.Size.IsSelected = true;

                await _cartService.UpsertAsync(CartItems.ToList());
                await LoadCartItemsAsync();
            }
        }

        [RelayCommand]
        private async Task IncreaseQuantityAsync(int productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == productId);

            if (cartItem == null)
            {
                var popup = new MessagePopup("Greška", "Došlo je do greške, molimo pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            else
            {
                cartItem.Quantity++;
                RefreshCartUI();
                await _cartService.UpsertAsync([.. CartItems]);
            }
        }

        [RelayCommand]
        private async Task DecreaseQuantityAsync(int productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == productId);

            if (cartItem == null)
            {
                var popup = new MessagePopup("Greška", "Došlo je do greške, molimo pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            else
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    RefreshCartUI();
                    await _cartService.UpsertAsync(CartItems.ToList());
                }
                else
                {
                    await RemoveCartItemAsync(productId);
                }
            }
        }

        [RelayCommand]
        private async Task RemoveCartItemAsync(int productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.Id == productId);

            if (cartItem == null)
            {
                var popup = new MessagePopup("Greška", "Došlo je do greške, molimo pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            else
            {
                CartItems.Remove(cartItem);
                await _cartService.RemoveItem(cartItem);
                RefreshCartUI();
            }            
        }

        [RelayCommand]
        private async Task ClearCartItemsAsync()
        {
            CartItems.Clear();
            await _cartService.ClearCart();
            RefreshCartUI();
        }

        [RelayCommand]
        private async Task Checkout()
        {
            try
            {
                var orderItems = CartItems.Where(x => x.IsSelected).Select(x => new CreateOrderItem()
                {
                    ProductColorId = x.Product.Id,
                    SizeId = x.Size.Id,
                    Quantity = x.Quantity
                }).ToList();

                await Shell.Current.GoToAsync(nameof(CheckoutPage), true, new Dictionary<string, object>
                {
                    { "OrderItems", orderItems },
                    { "TotalPrice", TotalPrice ?? 0 },
                    { "CartQuantity", CartQuantitySelectedItems }
                });
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }

        public async Task RemovePurchasedItemsAsync()
        {
            var selectedItems = CartItems.Where(x => x.IsSelected).ToList();

            foreach (var item in selectedItems)
                CartItems.Remove(item);

            await _cartService.RemoveItems(selectedItems);
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

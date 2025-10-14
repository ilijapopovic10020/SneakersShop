using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;

namespace SneakersShop.ViewModels
{
    [QueryProperty(nameof(OrderId), "OrderId")]
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            Order = new();
        }

        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        private OrderModel order;

        [ObservableProperty]
        private double widthProgress;

        [ObservableProperty]
        private Color pendingColor;

        [ObservableProperty]
        private Color shippedColor;

        [ObservableProperty]
        private Color deliveredColor;

        [ObservableProperty]
        private bool isLoading = false;

        [RelayCommand]
        private async Task LoadOrder()
        {
            try
            {
                IsLoading = true;

                Order = await _orderService.GetOrderByIdAsync(OrderId);

                UpdateOrderProgress();
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
        private async Task ProductDetail(int productId)
        {
            await Shell.Current.GoToAsync($"{nameof(ProductPage)}?ProductId={productId}");
        }

        private void UpdateOrderProgress()
        {
            WidthProgress = Order.OrderStatus switch
            {
                OrderStatus.Pending => 0.0,
                OrderStatus.Shipped => 150.0,
                OrderStatus.Delivered => 300.0,
                _ => 0.0
            };

            PendingColor = Order.OrderStatus >= OrderStatus.Pending ? Color.FromArgb("#FF9E6F") : Colors.LightGray;
            ShippedColor = Order.OrderStatus >= OrderStatus.Shipped ? Color.FromArgb("#FF7B40") : Colors.LightGray;
            DeliveredColor = Order.OrderStatus >= OrderStatus.Delivered ? Color.FromArgb("#FF5500") : Colors.LightGray;
        }
    }
}

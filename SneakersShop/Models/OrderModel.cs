using SneakersShop.Extensions;
using SneakersShop.Helpers;

namespace SneakersShop.Models
{
    public class OrdersModel : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrdersItemsModel> Items { get; set; } = [];

        public string OrderStatusDisplay => OrderStatus.GetDescription();

        public Color StatusColor =>
            OrderStatus switch
            {
                OrderStatus.Pending => Color.FromArgb("#FFFDE7"),
                OrderStatus.Shipped => Color.FromArgb("#B2DFDB"),
                OrderStatus.Delivered => Color.FromArgb("#C8E6C9"),
                OrderStatus.Cancelled => Color.FromArgb("#FFCDD2"),
                _ => Colors.Transparent,
            };

        public bool IsCancelVisible => OrderStatus == OrderStatus.Pending;
    }

    public class OrdersItemsModel
    {
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Size { get; set; }
        public int Quantity { get; set; }

        public string DisplaySize
        {
            get
            {
                if (Size % 1 == 0)
                    return ((int)Size).ToString();

                return Size.ToString("0.0");
            }
        }

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }

    public class OrderModel : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PromisedDate { get; set; }
        public string? EstimatedArrival { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrderItemModel> Items { get; set; } = [];
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string OrderStatusDisplay => OrderStatus.GetDescription();

        public Color StatusColor =>
            OrderStatus switch
            {
                OrderStatus.Pending => Color.FromArgb("#FFFDE7"),
                OrderStatus.Shipped => Color.FromArgb("#B2DFDB"),
                OrderStatus.Delivered => Color.FromArgb("#C8E6C9"),
                OrderStatus.Cancelled => Color.FromArgb("#FFCDD2"),
                _ => Colors.Transparent,
            };
    }

    public class OrderItemModel : BaseModel
    {
        public int ProductColorId { get; set; }
        public string Image { get; set; } = string.Empty;

        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }

    public class CreateOrderModel
    {
        public int PaymentType { get; set; }
        public string? Notes { get; set; }
        public string? CardHolder { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public string? Expiration { get; set; }
        public int AddressId { get; set; }

        public IEnumerable<CreateOrderItem>? Items { get; set; }
    }

    public class CreateOrderItem
    {
        public int ProductColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
    }
}

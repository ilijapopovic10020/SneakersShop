using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using System.Text.Json.Serialization;

namespace SneakersShop.MVVM.Models
{
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

    public class OrdersModel : BaseModel
    {
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrdersItemsModel> Items { get; set; }

        [JsonIgnore]
        public string OrderStatusDisplay => OrderStatus.GetDescription();

        [JsonIgnore]
        public Color StatusColor => OrderStatus switch
        {
            OrderStatus.Pending => Color.FromArgb("#FFFDE7"),
            OrderStatus.Shipped => Color.FromArgb("#B2DFDB"),
            OrderStatus.Delivered => Color.FromArgb("#C8E6C9"),
            OrderStatus.Cancelled => Color.FromArgb("#FFCDD2"),
            _ => Colors.Transparent
        };
    }

    public class OrdersItemsModel
    {
        public string Image { get; set; }

        [JsonIgnore]
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
        public IEnumerable<OrderItemModel> Items { get; set; }

        [JsonIgnore]
        public string OrderStatusDisplay => OrderStatus.GetDescription();

        [JsonIgnore]
        public Color StatusColor => OrderStatus switch
        {
            OrderStatus.Pending => Color.FromArgb("#FFFDE7"),     
            OrderStatus.Shipped => Color.FromArgb("#B2DFDB"),     
            OrderStatus.Delivered => Color.FromArgb("#C8E6C9"), 
            OrderStatus.Cancelled => Color.FromArgb("#FFCDD2"),
            _ => Colors.Transparent
        };

    }

    public class OrderItemModel : BaseModel
    {
        public int ProductColorId { get; set; }
        public string Image { get; set; }
        public decimal SizeNumber { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public string FullImageUrl => $"{AppConstants.IMAGE_URL}{Image}";
    }
}

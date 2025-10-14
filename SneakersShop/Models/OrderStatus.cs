using System.ComponentModel;

namespace SneakersShop.Models
{
    public enum OrderStatus
    {
        [Description("Sve")]
        All = 0,
        [Description("Na Čekanju")]
        Pending = 1,
        [Description("Isporučeno")]
        Shipped = 2,
        [Description("Preuzeto")]
        Delivered = 3,
        [Description("Obustavljeno")]
        Cancelled = 4
    }
}

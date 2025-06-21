using System.ComponentModel;

namespace SneakersShop.Helpers
{
    public enum OrderStatus
    {
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

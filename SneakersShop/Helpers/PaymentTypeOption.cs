using SneakersShop.Extensions;
using SneakersShop.Models;

namespace SneakersShop.Helpers
{
    public class PaymentTypeOption
    {
        public PaymentType PaymentType { get; set; }
        public string DisplayName => PaymentType.GetDescription();
    }
}

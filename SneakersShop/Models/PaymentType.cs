using System.ComponentModel;

namespace SneakersShop.Models
{
    public enum PaymentType
    {
        [Description("Plaćanje karticom")]
        Card = 1,

        [Description("Plaćanje pouzećem")]
        COD = 2
    }
}

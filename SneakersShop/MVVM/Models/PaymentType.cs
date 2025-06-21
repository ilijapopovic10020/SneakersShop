using System.ComponentModel;

namespace SneakersShop.MVVM.Models
{
    public enum PaymentType
    {
        [Description("Plaćanje karticom")]
        Card = 1,

        [Description("Plaćanje pouzećem")]
        COD = 2
    }
}

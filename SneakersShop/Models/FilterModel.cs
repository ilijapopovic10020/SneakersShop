using System.ComponentModel;

namespace SneakersShop.Models
{
    public enum FilterModel
    {
        [Description("Najprodavaniji")]
        BestSeller,

        [Description("Najnoviji")]
        Newest,

        [Description("Cena: rastuće")]
        PriceLowToHigh,

        [Description("Cena: opadajuće")]
        PriceHighToLow,

        [Description("Najbolje ocenjeni")]
        BestRated
    }
}

using System.ComponentModel;

namespace SneakersShop.Helpers
{
    public enum Filter
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

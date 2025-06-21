using CommunityToolkit.Mvvm.ComponentModel;

namespace SneakersShop.Helpers
{
    public partial class StarsModel : ObservableObject
    {
        public int Index { get; set; }

        [ObservableProperty]
        public bool isFilled;
    }
}

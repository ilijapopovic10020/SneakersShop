using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Helpers
{
    public partial class StarsModel : ObservableObject
    {
        public int Index { get; set; }

        [ObservableProperty]
        private bool isFilled;
    }
}

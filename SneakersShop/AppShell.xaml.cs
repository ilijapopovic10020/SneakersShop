using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Views;
using System.ComponentModel;

namespace SneakersShop
{
    public partial class AppShell : Shell
    {
        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                if (_isUserLoggedIn != value)
                {
                    _isUserLoggedIn = value;
                    OnPropertyChanged(nameof(IsUserLoggedIn));
                }
            }
        }

        public ShellContent FavoriteTab { get; private set; }

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;

            Routing.RegisterRoute(nameof(CreateAddressPage), typeof(CreateAddressPage));

            

            UpdateFavoriteTab();
        }

        private async void UpdateFavoriteTab()
        {
            try
            {
                var user = await SecureStorage.Default.GetUser();

                if (user != null)
                {
                    IsUserLoggedIn = true;
                }
                else
                {
                    IsUserLoggedIn = false;
                }

                if (IsUserLoggedIn)
                {
                    if (FavoriteTab == null)
                    {
                        FavoriteTab = new ShellContent
                        {
                            Title = "Omiljeno",
                            ContentTemplate = new DataTemplate(typeof(FavoritePage)),
                            Icon = "heart.svg",
                            Route = "FavoritePage"
                        };
                        bottomTabbar.Items.Insert(3, FavoriteTab);
                    }
                }
                else
                {
                    if (FavoriteTab != null && bottomTabbar.Items.Contains(FavoriteTab))
                    {
                        bottomTabbar.Items.Remove(FavoriteTab);
                        FavoriteTab = null;
                    }
                }
            }
            catch (Exception)
            {
                IsUserLoggedIn = false;

                if (FavoriteTab != null && bottomTabbar.Items.Contains(FavoriteTab))
                {
                    bottomTabbar.Items.Remove(FavoriteTab);
                    FavoriteTab = null;
                }
            }
            
        }
    }
}

using SneakersShop.Extensions;
using SneakersShop.Views;

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
                if(_isUserLoggedIn != value)
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

            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(FiltersPage), typeof(FiltersPage));

            Routing.RegisterRoute(nameof(AddressesPage), typeof(AddressesPage));
            Routing.RegisterRoute(nameof(AddressUpdatePage), typeof(AddressUpdatePage));
            Routing.RegisterRoute(nameof(AddressCreatePage), typeof(AddressCreatePage));

            Routing.RegisterRoute(nameof(ProfileUpdatePage), typeof(ProfileUpdatePage));

            Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
            Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));

            Routing.RegisterRoute(nameof(PasswordUpdatePage), typeof(PasswordUpdatePage));

            Routing.RegisterRoute(nameof(ContactPage), typeof(ContactPage));

            Routing.RegisterRoute(nameof(PolicyAndPrivacyPage), typeof(PolicyAndPrivacyPage));

            Routing.RegisterRoute(nameof(ReviewsPage), typeof(ReviewsPage));
            Routing.RegisterRoute(nameof(ReviewCreatePage), typeof(ReviewCreatePage));

            Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
            Routing.RegisterRoute(nameof(OrderSuccessPage), typeof(OrderSuccessPage));

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
                        FavoriteTab = new ShellContent()
                        {
                            ContentTemplate = new DataTemplate(typeof(FavoritesPage)),
                            Icon = "heart.png",
                            Route = "FavoritesPage",
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

using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Views;
using SneakersShop.Services;

namespace SneakersShop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;

            MainPage = new LoadingPage();

            ApplySavedTheme();
        }

        private static async void ApplySavedTheme()
        {
            await ThemeManager.LoadAndApplySavedThemeAsync();
        }

        protected async override void OnStart()            
        {
            base.OnStart();
            try
            {
                var user = await SecureStorage.Default.GetUser();

                if (user != null)
                {
                    if (user.ShouldBeLoggedOut)
                    {
                        SecureStorage.Default.Remove("user");

                        if (Application.Current != null)
                            Application.Current.MainPage = new WelcomePage();
                    }
                    else
                    {
                        var cartItemsFromServer = await CartService.Instance.Get();
                        CartService.Instance.LoadFromServer(cartItemsFromServer);

                        if (Application.Current != null)
                            Application.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    if (Application.Current != null)
                        Application.Current.MainPage = new WelcomePage();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n{e.InnerException?.Message}");
                if (Application.Current != null)
                    Application.Current.MainPage = new WelcomePage();
            }
        }
    }
}

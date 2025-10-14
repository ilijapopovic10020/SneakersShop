using SneakersShop.Extensions;
using SneakersShop.Views;

namespace SneakersShop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
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
                        await Shell.Current.GoToAsync($"{nameof(WelcomePage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    }
                }
                else
                {

                    await Shell.Current.GoToAsync($"{nameof(WelcomePage)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SecureStorage.Default.Remove("user");

                await Shell.Current.GoToAsync($"{nameof(WelcomePage)}");
            }
        }
    }
}

using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
//using SneakersShop.Platforms.Android;

namespace SneakersShop
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    { }
}

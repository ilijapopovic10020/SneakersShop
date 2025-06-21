using Microsoft.Extensions.Logging;
using UraniumUI;
using CommunityToolkit.Maui;
using Mopups.Hosting;
//#if ANDROID
//using SneakersShop.Platforms.Android;
//#elif IOS
//using SneakersShop.Platforms.iOS;
//#endif

namespace SneakersShop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
//                .ConfigureMauiHandlers(handlers =>
//                {
//#if ANDROID || IOS
//                    handlers.AddHandler<Shell, RoundedFloatingTabbarHandler>();
//#endif
//                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

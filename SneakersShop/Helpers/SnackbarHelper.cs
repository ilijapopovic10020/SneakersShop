using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SneakersShop.Helpers
{
    public static class SnackbarHelper
    {
        public static async Task ShowMessage(string message)
        {
            
            var isDarkMode = Application.Current.RequestedTheme == AppTheme.Dark;

            CancellationTokenSource cancellationTokenSource = new();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = isDarkMode ? Colors.White : Colors.Black,
                TextColor = isDarkMode ? Colors.Black : Colors.White,
                ActionButtonTextColor = isDarkMode ? Colors.Black : Colors.White,
                CornerRadius = new CornerRadius(10),
            };

            string actionButtonText = "Okay";
            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(message, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}

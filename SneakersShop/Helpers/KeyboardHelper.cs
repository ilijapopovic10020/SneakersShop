using Microsoft.Maui.Platform;
using System;

namespace SneakersShop.Helpers
{
    public static class KeyboardHelper
    {
        public static void HideKeyboard()
        {
#if ANDROID
            var context = Platform.CurrentActivity;
            var inputMethodManager = context?.GetSystemService(Android.Content.Context.InputMethodService) as Android.Views.InputMethods.InputMethodManager;
            var currentFocus = context?.CurrentFocus;
            if (inputMethodManager != null && currentFocus?.WindowToken != null)
            {
                inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, Android.Views.InputMethods.HideSoftInputFlags.None);
            }
#endif
        }
    }
}

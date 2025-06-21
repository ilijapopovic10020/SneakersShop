using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Helpers
{
    public static class ThemeManager
    {
        private const string ThemeKey = "Theme";
        public static async Task<AppVisualTheme> LoadAndApplySavedThemeAsync()
        {
            var savedThemeString = await SecureStorage.Default.GetAsync(ThemeKey);

            if (Enum.TryParse<AppVisualTheme>(savedThemeString, out var savedTheme))
            {
                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = savedTheme switch
                    {
                        AppVisualTheme.Light => AppTheme.Light,
                        AppVisualTheme.Dark => AppTheme.Dark,
                        _ => AppTheme.Unspecified
                    };
                }

                return savedTheme;
            }
            else
            {
                var systemTheme = AppInfo.Current.RequestedTheme;
                var fallbackTheme = systemTheme == AppTheme.Dark ? AppVisualTheme.Dark : AppVisualTheme.Light;

                if (Application.Current != null)
                    Application.Current.UserAppTheme = systemTheme;

                await SecureStorage.Default.SetAsync(ThemeKey, fallbackTheme.ToString());
                return fallbackTheme;
            }
        }

        public static async Task SaveTheme(AppVisualTheme theme)
        {
            await SecureStorage.Default.SetAsync(ThemeKey, theme.ToString());

            if(Application.Current != null)
            {
                Application.Current.UserAppTheme = theme switch
                {
                    AppVisualTheme.Light => AppTheme.Light,
                    AppVisualTheme.Dark => AppTheme.Dark,
                    _ => AppTheme.Unspecified
                };
            }
        }
    }
    }

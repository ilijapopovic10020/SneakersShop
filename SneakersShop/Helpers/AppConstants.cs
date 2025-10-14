namespace SneakersShop.Helpers
{
    public static class AppConstants
    {
        public static string API_URL =>
            IsRunningOnEmulator ? "http://10.0.2.2:5000" :
            IsWindows ? "http://localhost:5000" :
            "http://192.168.0.147:5000";

        public static string IMAGE_URL =>
            IsRunningOnEmulator ? "http://10.0.2.2:5000" :
            IsWindows ? "http://localhost:5000" :
            "http://192.168.0.147:5000";

        private static bool IsRunningOnEmulator =>
            DeviceInfo.DeviceType == DeviceType.Virtual;

        private static bool IsWindows =>
#if WINDOWS
                true;
#else
            false;
#endif
    }
}

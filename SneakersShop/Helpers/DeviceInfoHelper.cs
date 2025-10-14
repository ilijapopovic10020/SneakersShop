using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Helpers
{
    public static class DeviceInfoHelper
    {
        private const string Key = "device_id";

        public static async Task<string> GetDeviceInfoAsync()
        {
            var deviceId = await SecureStorage.Default.GetAsync(Key);

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
                await SecureStorage.Default.SetAsync(Key, deviceId);
            }

            var platfrom = DeviceInfo.Current.Platform.ToString();
            var manufacturer = DeviceInfo.Current.Manufacturer.ToString();
            var model = DeviceInfo.Current.Model.ToString();

            return $"{platfrom}-{manufacturer}-{model}-{deviceId}";
        }
    }
}

using System.ComponentModel;
using System.Reflection;

namespace SneakersShop.Extensions
{
    public static class EnumDescriptionExtension
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}

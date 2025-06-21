using Newtonsoft.Json;
using SneakersShop.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace SneakersShop.Helpers.Extensions
{
    public static class DateExtension
    {
        public static DateTime ToDateTime(this double unixTimeStamp)
        {
            DateTime dateTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime()!;
            return dateTime;
        }        
    }
}

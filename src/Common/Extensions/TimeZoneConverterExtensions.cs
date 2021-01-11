using Common.Constants;
using System;

namespace Common.Helpers
{
    public static class TimeZoneConverterExtensions
    {
        public static string ConvertToLocal(this DateTime utc)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(SystemNames.LocalTimeZone);
            DateTime local = TimeZoneInfo.ConvertTimeFromUtc(utc, zone);
            return local.ToString("dd/MM/yyyy HH:mm");
        }

        public static DateTime ConvertToUtc(this DateTime local)
        {
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(SystemNames.LocalTimeZone);
            DateTime utc = TimeZoneInfo.ConvertTimeToUtc(local, zone);
            return utc;
        }
    }
}

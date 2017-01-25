using System;

namespace TodoScheduler.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hour, int minutes, int seconds)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minutes, seconds, DateTimeKind.Utc);
        }

    }
}

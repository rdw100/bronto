namespace Bronto.WebApi.Framework
{
    using System;

    public class UnixTimestampCalculator
    {
        public long GetMondayUnixTimestamp(DateTime dateTime)
        {
            // Find the most recent Monday (or today if it's Monday)
            DateTime monday = dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Monday);
            
            // Convert to Unix timestamp
            return ((DateTimeOffset)monday).ToUnixTimeSeconds();
        }

        public long GetFridayUnixTimestamp(DateTime dateTime)
        {
            // Find the most recent Monday (or today if it's Monday)
            DateTime monday = dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Monday);

            // Calculate the Unix timestamp for Friday (5 days after Monday)
            DateTime friday = monday.AddDays(4);

            // Convert to Unix timestamp
            return ((DateTimeOffset)friday).ToUnixTimeSeconds();
        }
    }
}

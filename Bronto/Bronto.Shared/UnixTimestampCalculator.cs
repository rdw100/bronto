namespace Bronto.Shared
{
    using System;

    /// <summary>
    /// Calculates the Unix timestamp conversions, based on seconds, for date computation.
    /// </summary>
    public class UnixTimestampCalculator
    {
        public long MondayUnixTime { get; private set; }
        public long FridayUnixTime { get; private set; }

        public UnixTimestampCalculator()
        {
                
        }

        public UnixTimestampCalculator(DateTime dateTime)
        {
            MondayUnixTime = GetMondayUnixTimestamp(dateTime);
            FridayUnixTime = GetFridayUnixTimestamp(dateTime);
        }

        /// <summary>
        /// Calculates the most recent Monday.
        /// </summary>
        /// <param name="dateTime">A date and time of day.</param>
        /// <returns>Returns the most recent Monday.</returns>
        public long GetMondayUnixTimestamp(DateTime dateTime)
        {
            // Find the most recent Monday (or today if it's Monday)
            //DateTime monday = dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime monday = GetMostRecentMonday(dateTime);

            // Convert to Unix timestamp
            //return ((DateTimeOffset)monday).ToUnixTimeSeconds();
            return ToUnixTime(monday);
        }

        /// <summary>
        /// Calculates Friday, based on recent Monday.
        /// </summary>
        /// <param name="dateTime">A date and time of day.</param>
        /// <returns>Returns the most recent Friday.</returns>
        public long GetFridayUnixTimestamp(DateTime dateTime)
        {
            // Find the most recent Monday (or today if it's Monday)
            //DateTime monday = dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime monday = GetMostRecentMonday(dateTime);

            // Calculate the Unix timestamp for Friday (5 days after Monday)
            DateTime friday = monday.AddDays(4);

            // Convert to Unix timestamp
            //return ((DateTimeOffset)friday).ToUnixTimeSeconds();
            return ToUnixTime(friday);
        }

        private DateTime GetMostRecentMonday(DateTime currentDate)
        {
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }
            return currentDate.Date.AddDays(-daysToSubtract);
        }

        public static long ToUnixTime(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }
    }
}
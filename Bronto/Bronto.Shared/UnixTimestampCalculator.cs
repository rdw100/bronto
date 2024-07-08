namespace Bronto.Shared
{
    using System;

    /// <summary>
    /// Calculates the Unix timestamp conversions, based on seconds, for date computation.
    /// </summary>
    public class UnixTimestampCalculator
    {
        /// <summary>
        /// Calculates the most recent Monday.
        /// </summary>
        /// <param name="dateTime">A date and time of day.</param>
        /// <returns>Returns the most recent Monday.</returns>
        public long GetMondayUnixTimestamp(DateTime dateTime)
        {
            // Find the most recent Monday (or today if it's Monday)
            DateTime monday = dateTime.AddDays(-(int)dateTime.DayOfWeek + (int)DayOfWeek.Monday);

            // Convert to Unix timestamp
            return ((DateTimeOffset)monday).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Calculates Friday, based on recent Monday.
        /// </summary>
        /// <param name="dateTime">A date and time of day.</param>
        /// <returns>Returns the most recent Friday.</returns>
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
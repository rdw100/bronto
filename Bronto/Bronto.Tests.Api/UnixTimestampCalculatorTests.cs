using Bronto.Shared;

namespace Bronto.Tests.Api
{
    public class UnixTimestampCalculatorTests
    {
        private readonly UnixTimestampCalculator _calculator;

        public UnixTimestampCalculatorTests()
        {
            // Initialize the calculator (you can also use a fixture if needed)
            _calculator = new UnixTimestampCalculator();
        }

        [Fact]
        public void GetMondayUnixTimestamp_CurrentWeek_ReturnsMondayTimestamp()
        {
            // Arrange: Set the current date to a Monday
            DateTime monday = new DateTime(2024, 7, 15); // Adjust as needed
            long expectedMondayTimestamp = 1721016000; // Monday, July 15, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetMondayUnixTimestamp(monday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedMondayTimestamp, timestamp);
        }

        [Fact]
        public void GetMondayUnixTimestamp_CurrentSunday_ReturnsMondayTimestamp()
        {
            // Arrange: Set the current date to a Sunday
            DateTime sunday = new DateTime(2024, 7, 7); // Adjust as needed
            long expectedMondayTimestamp = 1719806400; // Sunday, July 7, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetMondayUnixTimestamp(sunday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedMondayTimestamp, timestamp);
        }

        [Fact]
        public void GetFridayUnixTimestamp_CurrentWeek_ReturnsFridayTimestamp()
        {
            // Arrange: Set the current date to a Friday
            DateTime friday = new DateTime(2024, 7, 19); // Adjust as needed
            long expectedFridayTimestamp = 1721361600; // Friday, July 19, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetFridayUnixTimestamp(friday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedFridayTimestamp, timestamp);
        }

        [Fact]
        public void GetFridayUnixTimestamp_CurrentWeekThursday_ReturnsFridayTimestamp()
        {
            // Arrange: Set the current date to a Friday
            DateTime friday = new DateTime(2024, 7, 18); // Adjust as needed
            long expectedFridayTimestamp = 1721361600; // Friday, July 19, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetFridayUnixTimestamp(friday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedFridayTimestamp, timestamp);
        }

        [Fact]
        public void GetMondayUnixTimestamp_PreviousWeek_ReturnsMondayTimestamp()
        {
            // Arrange: Set the current date to a Monday
            DateTime monday = new DateTime(2024, 7, 6); // Adjust as needed
            long expectedMondayTimestamp = 1719806400; // Monday, July 1, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetMondayUnixTimestamp(monday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedMondayTimestamp, timestamp);
        }

        [Theory]
        [InlineData("2024-07-07", 1720152000)] // Sunday 7/7 retrieves last week 7/1 - 7/5.
        [InlineData("2024-07-06", 1720152000)] // Saturday 7/7 retrieves last week 7/1 - 7/5.
        public void GetFridayUnixTimestamp_PreviousWeek_ReturnsFridayTimestamp(DateTime day, long expected)
        {
            // Arrange: Set the current date to a Friday
            DateTime friday = day; // Adjust as needed
            long expectedFridayTimestamp = expected; // Friday, July 5, 2024 12:00:00 AM GMT-04:00 DST

            // Act
            long timestamp = _calculator.GetFridayUnixTimestamp(friday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedFridayTimestamp, timestamp);
        }
    }
}

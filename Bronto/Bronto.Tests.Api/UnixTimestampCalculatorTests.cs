using Bronto.WebApi.Framework;

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
            long expectedMondayTimestamp = 1721016000; // Mon Jul 15 2024 04:00:00 GMT+0000

            // Act
            long timestamp = _calculator.GetMondayUnixTimestamp(monday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedMondayTimestamp, timestamp);
        }

        [Fact]
        public void GetFridayUnixTimestamp_CurrentWeek_ReturnsFridayTimestamp()
        {
            // Arrange: Set the current date to a Friday
            DateTime friday = new DateTime(2024, 7, 19); // Adjust as needed
            long expectedFridayTimestamp = 1721361600; // Fri Jul 19 2024 04:00:00 GMT+0000

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
            long expectedFridayTimestamp = 1721361600; // Fri Jul 19 2024 04:00:00 GMT+0000

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
            long expectedMondayTimestamp = 1719806400; // Mon Jul 01 2024 04:00:00 GMT+0000

            // Act
            long timestamp = _calculator.GetMondayUnixTimestamp(monday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedMondayTimestamp, timestamp);
        }

        [Fact]
        public void GetFridayUnixTimestamp_PreviousWeek_ReturnsFridayTimestamp()
        {
            // Arrange: Set the current date to a Friday
            DateTime friday = new DateTime(2024, 7, 7); // Adjust as needed
            long expectedFridayTimestamp = 1720756800; // Fri Jul 12 2024 04:00:00 GMT+0000

            // Act
            long timestamp = _calculator.GetFridayUnixTimestamp(friday);

            // Assert: Verify the expected Unix timestamp
            Assert.Equal(expectedFridayTimestamp, timestamp);
        }
    }
}

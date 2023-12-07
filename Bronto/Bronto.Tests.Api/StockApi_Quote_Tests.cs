using FluentAssertions;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.Configuration;
using Bronto.Tests.Api.Models;
using Bronto.Models.Api.Price.Response;

namespace Bronto.Tests.Api
{

    public class StockApi_GetQuote_Tests : IClassFixture<StockFixture>
    {
        private readonly StockFixture _fixture;

        public StockApi_GetQuote_Tests(StockFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void StockApiClient_ShouldGetTimeSeriesAsync_ReturnsTrue()
        {
            // ARRANGE
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When($"https://{_fixture.Host}/*")
                .Respond("application/json", "{\"meta\":{\"symbol\":\"AAPL\",\"interval\":\"1min\",\"currency\":\"USD\",\"exchange_timezone\":\"America/New_York\",\"exchange\":\"NASDAQ\",\"type\":\"Common Stock\"},\"values\":[{\"datetime\":\"2023-12-01 00:00:00\",\"open\":\"191.13000\",\"high\":\"191.24500\",\"low\":\"191.12700\",\"close\":\"191.24500\",\"volume\":\"44707\"}],\"status\":\"ok\"}");

            StockApiClient stockApiClient = new StockApiClient(_fixture.Key, mockHttp.ToHttpClient());

            // ACT
            var response = await stockApiClient.GetTimeSeriesAsync("AAPL");

            // ASSERT
            response?.ResponseStatus.Should().Be(Enums.StockDataClientResponseStatus.Ok);
            response?.ResponseMessage.Should().Be("RESPONSE_OK");
            response?.Values[0]?.Datetime.Should().Be(new DateTime(2023, 12, 1, 00, 00, 00));
            response?.ExchangeTimezone.Should().Be("America/New_York");
            response?.Exchange.Should().Be("NASDAQ");
            response?.Type.Should().Be("Common Stock");
            response?.Values[0]?.Open.Should().Be(191.13000);
            response?.Values[0]?.High.Should().Be(191.24500);
            response?.Values[0]?.Low.Should().Be(191.12700);
            response?.Values[0]?.Close.Should().Be(191.24500);
            response?.Values[0]?.Volume.Should().Be(44707);
        }

        [Fact]
        public async void StockApi_ShouldGetStockPriceAsync_ReturnsTrue()
        {
            // Arrange
            var client = new HttpClient();
            var url = $"https://{_fixture.Host}/price?symbol=AAPL&apikey={_fixture.Key}";

            // Act
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(content);
            Assert.Contains("price", content);
            Assert.True(response.IsSuccessStatusCode);
            content.Should().NotBeNull();
            content.Should().Contain("price");
        }

        [Fact]
        public async void StockApiClient_ShouldGetStockPriceAsync_ReturnsTrue()
        {
            // ARRANGE
            var mockHttp = new MockHttpMessageHandler();

            mockHttp
                .When($"https://{_fixture.Host}/*")
                .Respond("application/json", "{\"price\":\"193.07001\"}");

            StockApiClient stockApiClient = new StockApiClient(_fixture.Key, mockHttp.ToHttpClient());

            // ACT
            RealTimePrice response = await stockApiClient.GetRealTimePriceAsync("AAPL");

            // ASSERT
            double price;
            if (double.TryParse(response.Price, out price))
            {
                price.Should().BeGreaterThan(0);
                price.Should().BePositive();
                price.Should().Be(193.07001);
            }
            else
            {
                Assert.NotNull(response);
            }
        }
    }

    /// <summary>
    /// Creates a single test context and share it among all the tests in the 
    /// class, and have it cleaned up after all the tests in the class have 
    /// finished.
    /// </summary>
    public class StockFixture : IDisposable
    {
        protected internal string Key { get; set; }
        protected internal string Host { get; set; }
        private ConfigurationBuilder builder { get; set; }
        private IConfiguration config { get; set; }

        /// <summary>
        /// Initialize variables for shared object instance.
        /// </summary>
        public StockFixture()
        {
            builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
        }

        /// <summary>
        /// Dispose of shared object instance variables.
        /// </summary>
        public void Dispose()
        {
            builder = null;
            config = null;
        }
    }
}
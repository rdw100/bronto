using FluentAssertions;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.Configuration;
using Bronto.Tests.Api.Models;

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
        public async void StockApi_ShouldGetTimeSeriesAsync_ReturnsTrue()
        {
            // ARRANGE
            var keyString = _fixture.Key.ToString();
            var hostString = _fixture.Host.ToString();
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
            var keyString = _fixture.Key.ToString();
            var hostString = _fixture.Host.ToString();
            var client = new HttpClient();
            var url = $"https://{_fixture.Host}/price?symbol=AAPL&apikey={_fixture.Key}&source=docs";

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
    }

    public class StockFixture : IDisposable
    {
        public string Key { get; set; }
        public string Host { get; set; }

        public StockFixture()
        {
            // Do global initialization here  
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
        }

        public void Dispose()
        {
            // Do global teardown here
        }
    }
}
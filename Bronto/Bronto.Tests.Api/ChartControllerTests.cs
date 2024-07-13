using Bronto.Models.Api.Chart;
using Bronto.WebApi.Controllers;
using Bronto.WebApi.Services;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Bronto.Tests.Api
{
    public class ChartControllerTests
    {
        private readonly IConfiguration _testConfiguration;
        private readonly IHttpService _httpService;
        private readonly IMemoryCache _cache;
        private readonly IChartService _chartService;
        private readonly ChartController _chartController;

        public ChartControllerTests()
        {
            _testConfiguration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _chartService = new ChartService(_testConfiguration, _cache, _httpService);
            _chartController = new ChartController(_testConfiguration, _cache, _chartService);
        }

        [Theory]
        [InlineData("AAPL")]
        [InlineData("NVDA")]
        public async Task GetStockDataAsync_ShouldReturnChartResult(string symbol)
        {
            // Act: Call the Get method
            var result = await _chartController.GetChartData(symbol);

            // Assert: Verify the result
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ohlcList = Assert.IsType<ChartResult>(okResult.Value);

            ohlcList.Chart.Result[0].Indicators.Quote.Count.Should().BePositive();
            ohlcList.Chart.Result[0].Indicators.Quote[0].Open[0].Should().BePositive();
            ohlcList.Chart.Result[0].Indicators.Quote[0].High[0].Should().BePositive();
            ohlcList.Chart.Result[0].Indicators.Quote[0].Low[0].Should().BePositive();
            ohlcList.Chart.Result[0].Indicators.Quote[0].Close[0].Should().BePositive();
        }
    }
}
using Bronto.Models;
using Bronto.WebApi.Controllers;
using Bronto.WebApi.Interfaces;
using Bronto.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Bronto.Tests.Api
{
    public class ChartControllerTests
    {
        private readonly IConfiguration _testConfiguration;
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
            _chartService = new ChartService(_testConfiguration, _cache);
            _chartController = new ChartController(_testConfiguration, _cache, _chartService);
        }

        [Theory]
        [InlineData("AAPL")]
        [InlineData("NVDA")]
        public async Task GetStockDataAsync_ShouldReturnOhlcList(string symbol)
        {
            // Act: Call the Get method
            var result = await _chartController.GetStockData(symbol);

            // Assert: Verify the result
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ohlcList = Assert.IsType<List<MyOHLC>>(okResult.Value);

            ohlcList.Count.Should().BePositive();
            ohlcList[0].Open.Should().BePositive();
            ohlcList[0].High.Should().BePositive();
            ohlcList[0].Low.Should().BePositive();
            ohlcList[0].Close.Should().BePositive();
        }
    }
}
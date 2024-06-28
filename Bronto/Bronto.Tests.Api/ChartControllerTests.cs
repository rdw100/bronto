using Bronto.Models;
using Bronto.WebApi.Controllers;
using Bronto.WebApi.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bronto.Tests.Api
{
    public class ChartControllerTests
    {
        private readonly IConfiguration _testConfiguration;
        private readonly ChartController _chartController;

        public ChartControllerTests()
        {
            _testConfiguration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            _chartController = new ChartController(_testConfiguration);
        }

        [Fact]
        public async Task GetStockDataAsync_ShouldReturnOhlcList()
        {
            // Arrange: Set up context
            var symbol = "AAPL";

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
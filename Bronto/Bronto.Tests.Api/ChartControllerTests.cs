using Bronto.Models.Api.Chart;
using Bronto.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

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
        public async Task GetStockDataAsync_ShouldReturnStock()
        {
            // Arrange: Set up context
            var symbol = "AAPL";

            // Act: Call the Get method
            var result = await _chartController.GetStockData(symbol);

            // Assert: Verify the result
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var content = okResult.Value.ToString();
            ChartResult newResult = JsonSerializer.Deserialize<ChartResult>(content);
            var actual = Assert.IsType<ChartResult>(newResult);
            actual.Chart.Result.Should().NotBeNull();
            List<Result> result1 = actual.Chart.Result;
            result1.Count.Should().BeGreaterThanOrEqualTo(1);
            List<Quote> quote1 = actual.Chart.Result[0].Indicators.Quote;
            quote1.Count.Should().BeGreaterThanOrEqualTo(1);
            List<double> open1 = actual.Chart.Result[0].Indicators.Quote[0].Open;
            open1[0].Should().BeGreaterThanOrEqualTo(1);
            open1[0].Should().BePositive();
        }
    }
}

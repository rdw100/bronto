using Bronto.Models.Api;
using Bronto.WebApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Bronto.Models.Api.Enums;

namespace Bronto.Tests.Api
{
    public class PriceControllerTests
    {
        private readonly IConfiguration _testConfiguration;
        private readonly PriceController _priceController;

        public PriceControllerTests()
        {
            _testConfiguration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            _priceController = new PriceController(_testConfiguration);
        }

        [Fact]
        public async Task GetStockPriceAsync_ShouldReturnStock()
        {
            // Arrange: Set up context
            var symbol = "AAPL";

            // Act: Call the Get method
            var result = await _priceController.Get(symbol);

            // Assert: Verify the result
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPrice = Assert.IsType<RealTimePrice>(okResult.Value);                        
            actualPrice.ResponseMessage.Should().Be("RESPONSE_OK");
            actualPrice.Price.Should().BeGreaterThan(0);
            actualPrice.Price.Should().BePositive();
            Assert.Equal(StockDataClientResponseStatus.Ok, actualPrice.ResponseStatus);
        }
    }
}
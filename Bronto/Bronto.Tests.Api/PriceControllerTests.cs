using Bronto.Models.Api;
using Bronto.WebApi.Controllers;
using Bronto.WebApi.Services;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using static Bronto.Models.Api.Enums;

namespace Bronto.Tests.Api
{
    public class PriceControllerTests
    {
        private readonly IConfiguration testConfiguration;
        private readonly PriceController priceController;
        private readonly IPriceService priceService;
        private readonly IMemoryCache iCache;
        private readonly ITwelveHttpService iTwelveHttpService;

        public PriceControllerTests()
        {
            testConfiguration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            iCache = new MemoryCache(new MemoryCacheOptions());
            iTwelveHttpService = new TwelveHttpService(new HttpClient(), testConfiguration);
            priceService = new PriceService(testConfiguration, iCache, iTwelveHttpService);
            priceController = new PriceController(testConfiguration, iCache, priceService);
        }

        [Fact]
        public async Task GetStockPriceAsync_ShouldReturnStock()
        {
            // Arrange: Set up context
            var symbol = "AAPL";

            // Act: Call the Get method
            var result = await priceController.Get(symbol);

            // Assert: Verify the result
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPrice = Assert.IsType<RealTimePrice>(okResult.Value);                        
            actualPrice.StatusMessage.Should().Be("RESPONSE_OK");
            actualPrice.Price.Should().BeGreaterThan(0);
            actualPrice.Price.Should().BePositive();
            Assert.Equal(StockDataClientResponseStatus.Ok, actualPrice.StatusCodeType);
        }
    }
}
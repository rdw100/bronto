using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Bronto.Tests.Api
{
    /// <summary>
    /// Supports integration tests using a unit test framework with a 
    /// test web host and an in-memory test server.
    /// </summary>
    public class ChartApi_StockData_Tests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ChartApi_StockData_Tests(WebApplicationFactory<Program> factory)
        {
            // Arrange
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:7048/")
            });
        }

        [Theory]
        [InlineData("AAPL")] // Test with a valid stock symbol
        public async Task GetChartData_ValidSymbol_ReturnOhlcOK(string symbol)
        {
            // Act
            var response = await _client.GetAsync($"/api/Chart?symbol={symbol}&interval=1d&range=5d");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content); // Ensure non-null response content
        }

        [Theory]
        [InlineData("INVALID")] // Test with an invalid stock symbol
        public async Task GetChartData_InvalidSymbol_ReturnsNotFound(string symbol)
        {
            // Act
            var response = await _client.GetAsync($"/api/Chart?symbol={symbol}&interval=1d&range=5d");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content); // Ensure non-null response content
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Status code 404
        }

        [Theory]
        [InlineData("NVDA")] // Test with an invalid stock symbol
        public async Task GetChartData_ValidSymbol_ReturnsOK(string symbol)
        {
            // Act
            var response = await _client.GetAsync($"/api/Chart?symbol={symbol}&interval=1d&range=5d");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content); // Ensure non-null response content
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); // Status code 200
        }

        [Theory]
        [InlineData("NFLX")]
        public async Task GetChartData_ValidSymbol_ReturnsOkResult(string symbol)
        {
            // Act
            var response = await _client.GetAsync($"/api/Chart?symbol={symbol}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(symbol, content);
        }
    }
}
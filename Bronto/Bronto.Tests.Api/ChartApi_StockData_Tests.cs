﻿using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Bronto.Tests.Api
{
    /// <summary>
    /// Supports integration tests using a unit test framework with a 
    /// test web host and an in-memory test server.
    /// </summary>
    public class ChartApi_StockData_Tests : IClassFixture<ApiTestFixture>
    {
        private readonly HttpClient _client;

        public ChartApi_StockData_Tests(ApiTestFixture fixture)
        {
            // Arrange
            _client = fixture.Client;
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

        [Theory]
        [InlineData("AAPL","1d","5d")] // Test with a valid stock symbol
        public async Task GetChartData_ValidSymbolIntervalRange_ReturnsOkResult(string symbol, string interval, string range)
        {
            // Act
            //var response = await _client.GetAsync($"/api/Chart?symbol={symbol}&interval={interval}&range={range}");
            var response = await _client.GetAsync($"/api/Chart?symbol={symbol}&interval=1d&range=5d");
            
            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(symbol, content);
            Assert.Contains(interval, content);
            Assert.Contains(range, content);
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
    }
}
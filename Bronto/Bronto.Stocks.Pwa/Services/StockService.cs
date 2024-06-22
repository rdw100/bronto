using Bronto.Models.Api;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;
using static Bronto.Models.Api.Enums;

namespace Bronto.Stocks.Pwa.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private StockDataTimeSeries stockDataList;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval, string outputsize)
        {
            var response = await _httpClient.GetAsync($"api/TimeSeries?symbol={symbol}&interval={interval}&outputsize={outputsize}");

            if (response.IsSuccessStatusCode)
            {
                stockDataList  = await response.Content.ReadFromJsonAsync<StockDataTimeSeries>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                // Handle 429 Too Many Requests
                return new StockDataTimeSeries
                {
                    ResponseMessage = "Rate limit exceeded. Please wait a while before making more requests.",
                    ResponseStatus = StockDataClientResponseStatus.RateLimitExceeded
                };
            }
            else
            {
                return new StockDataTimeSeries
                {
                    ResponseMessage = $"Stock API Error: {response.StatusCode}",
                    ResponseStatus = StockDataClientResponseStatus.StockDataApiError
                };
            }

            return stockDataList;
        }
    }
}
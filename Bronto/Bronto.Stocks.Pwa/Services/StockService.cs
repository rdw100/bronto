using Bronto.Models.Api;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;

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
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }

            return stockDataList;
        }
    }
}
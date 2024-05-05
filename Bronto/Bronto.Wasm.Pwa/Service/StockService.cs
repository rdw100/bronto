using Bronto.Models.Api;
using System.Net.Http.Json;

namespace Bronto.Wasm.Pwa.Service
{
    public class StockService
    {
        private readonly HttpClient _httpClient;
        private StockDataTimeSeries stockDataList;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetStockPriceAsync(string tickerSymbol)
        {
            Random rnd = new Random();
            return await Task.FromResult<decimal>(rnd.Next(5000, 20000) / 100);
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
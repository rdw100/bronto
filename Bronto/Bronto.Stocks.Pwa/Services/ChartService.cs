using Bronto.Models;
using Bronto.Models.Api.Chart;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;
using static Bronto.Models.Api.Enums;

namespace Bronto.Stocks.Pwa.Services
{
    public class ChartService : IChartService
    {
        private readonly HttpClient _httpClient;

        public ChartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MyOHLC>> GetStockData(string symbol)
        {
            var myOhlcList = new List<MyOHLC>();

            try
            {
                var response = await _httpClient.GetAsync($"api/Chart?symbol={symbol}&interval=1d&range=5d");

                if (response.IsSuccessStatusCode)
                {
                    myOhlcList = await response.Content.ReadFromJsonAsync<List<MyOHLC>>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {// Handle 429 Too Many Requests
                    var newItem = new MyOHLC
                    {
                        ResponseMessage = "Rate limit exceeded. Please wait a while before making more requests.",
                        ResponseStatus = StockDataClientResponseStatus.RateLimitExceeded
                    };

                    myOhlcList.Add(newItem);

                    return myOhlcList;
                }
                else
                {
                    var newItem = new MyOHLC
                    {
                        ResponseMessage = $"Stock API Error: {response.StatusCode}",
                        ResponseStatus = StockDataClientResponseStatus.StockDataApiError
                    };

                    myOhlcList.Add(newItem);

                    return myOhlcList;
                }

                return myOhlcList;
            }
            catch (HttpRequestException ex)
            {
                var newItem = new MyOHLC
                {
                    ResponseMessage = $"HTTP Error: {ex.Message}",
                    ResponseStatus = StockDataClientResponseStatus.StockDataError
                };

                myOhlcList.Add(newItem);

                return myOhlcList;
            }
        }

        public async Task<ChartResult> GetChartData(string symbol, string interval, string range)
        {
            var chart = new ChartResult();

            try
            {
                var response = await _httpClient.GetAsync($"stock/Chart?symbol={symbol}&interval={interval}&range={range}");

                if (response.IsSuccessStatusCode)
                {
                    chart = await response.Content.ReadFromJsonAsync<ChartResult>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    // Handle 429 Too Many Requests
                    return new ChartResult
                    {
                        ResponseMessage = "Rate limit exceeded. Please wait a while before making more requests.",
                        ResponseStatus = StockDataClientResponseStatus.RateLimitExceeded
                    };
                }
                else
                {
                    return new ChartResult
                    {
                        ResponseMessage = $"Stock API Error: {response.StatusCode}",
                        ResponseStatus = StockDataClientResponseStatus.StockDataApiError
                    };
                }

                return chart;
            }
            catch (HttpRequestException ex)
            {
                return new ChartResult
                {
                    ResponseMessage = $"HTTP Error: {ex.Message}",
                    ResponseStatus = StockDataClientResponseStatus.StockDataError
                };
            }
        }
    }
}
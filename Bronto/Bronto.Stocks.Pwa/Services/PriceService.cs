using Bronto.Models.Api;
using Bronto.Stocks.Pwa.Interfaces;
using System.Net.Http.Json;
using System.Web.Http;

namespace Bronto.Stocks.Pwa.Services
{
    public class PriceService : IPriceService
    {
        private readonly HttpClient _httpClient;
        private RealTimePrice price;

        public PriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RealTimePrice> GetPriceAsync(string symbol)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Price?symbol={symbol}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RealTimePrice>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    // Handle 429 Too Many Requests
                    throw new HttpRequestException("Rate limit exceeded. Please try again later.");
                }
                else
                {
                    Console.WriteLine($"Stock API Error: {response.StatusCode}");
                    // Handle other errors
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                // Log the exception or handle it accordingly
                throw new Exception($"An error occurred while fetching the stock price: {ex.Message}");
            }
        }
    }
}

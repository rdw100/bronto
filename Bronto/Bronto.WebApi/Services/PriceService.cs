using Bronto.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace Bronto.WebApi.Services
{
    public class PriceService
    {
        private readonly HttpClient _httpClient;

        private IConfiguration config { get; set; }

        protected internal string Key { get; set; }
        protected internal string Host { get; set; }

        public PriceService(IConfiguration iConfig)
        {
            config = iConfig;
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{Host}/");
        }

        public async Task<IActionResult> GetPriceAsync(string symbol)
        {
            return null;
            //try
            //{
            //    // Replace with the actual API endpoint for stock data
            //    string apiUrl = $"price?symbol={symbol}&apikey={Key}";

            //    // Make an asynchronous GET request to the API
            //    var response = await _httpClient.GetAsync(apiUrl);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        // Deserialize the stock data (assuming it's in JSON format)
            //        var stockData = await response.Content.ReadFromJsonAsync<RealTimePrice>();

            //        // Process the stock data as needed (e.g., extract relevant info)

            //        // Return an OkResult with the stock data
            //        return Ok(stockData);
            //    }
            //    else
            //    {
            //        // Handle non-successful response (e.g., log, return error status)
            //        return StatusCode((int)response.StatusCode);
            //    }
            //}
            //catch (HttpRequestException ex)
            //{
            //    // Handle exceptions (e.g., network issues, invalid URL)
            //    return BadRequest($"Error fetching stock data: {ex.Message}");
            //}
        }
    }
}

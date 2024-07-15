using Bronto.Models;
using Bronto.Models.Api.Chart;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace Bronto.WebApi.Services
{
    public class ChartService : IChartService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpService HttpService;
        private IConfiguration _config { get; set; }
        private readonly HttpClient _httpClient;
        protected internal string? _baseUrl { get; set; }

        public ChartService(IConfiguration iConfig, IMemoryCache cache, IHttpService httpService)
        {
            _config = iConfig;
            _cache = cache;
            _baseUrl = _config.GetSection("FreeApiV8")["Host"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{_baseUrl}/");
            _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            HttpService = httpService;
        }

        [HttpGet]
        public async Task<ChartResult> GetChartData(
            string symbol,
            string interval = "1d", // Default interval is 1 day
            string range = "5d",   // Default range is 5 days
            long? period1 = null,  // Default period1 is null (to be calculated)
            long? period2 = null)  // Default period2 is null (to be calculated)
        {
            // Construct the API URL//{_baseUrl}
            var apiUrl = $"{symbol}?interval={interval}&range={range}&period1={period1}&period2={period2}";
            var cacheKey = symbol + interval + range;

            try
            {
                if (!_cache.TryGetValue(cacheKey, out ChartResult chart))
                {
                    var response = await _httpClient.GetAsync(apiUrl);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response and create a Chart object
                        var data = await response.Content.ReadAsStringAsync();
                        chart = ParseJsonToChart(data);

                        // Cache the data for future requests
                        _cache.Set(cacheKey, chart, cacheEntryOptions);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Returns a 404 Not Found response
                        return null;
                    }
                    else
                    {
                        // Handle non-success status codes (e.g., log, throw exception, etc.)
                        throw new Exception($"HTTP request failed with status code {response.StatusCode}");
                    }
                }

                return chart;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching stock data: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses the JSON response into an ChartResult List
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        private ChartResult ParseJsonToChart(string jsonResponse)
        {
            var parsedData = JsonSerializer.Deserialize<ChartResult>(jsonResponse);
            return parsedData;
        }

        /// <summary>
        /// Parses the JSON response into an OHLC List
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        private List<MyOHLC> ParseJsonToOHLC(string jsonResponse)
        {
            var parsedData = JsonSerializer.Deserialize<ChartResult>(jsonResponse);
            
            List<Result>? tsStockValues = parsedData?.Chart.Result;
            
            var ohlcList = new List<MyOHLC>();

            for (int i = 0; i < tsStockValues[0].Indicators.Quote[0].Open.Count; i++)
            {
                var ohlc = new MyOHLC
                {
                    DateTime = UnixTimeStampToDateTime(tsStockValues[0].Timestamp[i]),
                    Open = tsStockValues[0].Indicators.Quote[0].Open[i],
                    High = tsStockValues[0].Indicators.Quote[0].High[i],
                    Low = tsStockValues[0].Indicators.Quote[0].Low[i],
                    Close = tsStockValues[0].Indicators.Quote[0].Close[i],
                    TimeSpan = TimeSpan.FromDays(1.0)
                };
                ohlcList.Add(ohlc);
            }

            return ohlcList;
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch (January 1, 1970)
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime = epoch.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static List<DateTime> UnixTimeStampsToDateTimeList(List<long> unixTimeStamps)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dateTimeList = new List<DateTime>();

            foreach (var unixTimeStamp in unixTimeStamps)
            {
                var dateTime = epoch.AddSeconds(unixTimeStamp).ToLocalTime();
                dateTimeList.Add(dateTime);
            }

            return dateTimeList;
        }
    }
}

using Bronto.Models.Api.Chart;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Bronto.WebApi.Services
{
    public class ChartService : IChartService
    {
        private readonly IMemoryCache cache;
        private readonly IHttpService httpService;
        private IConfiguration config { get; set; }

        public ChartService(IConfiguration iConfig, IMemoryCache iCache, IHttpService iHttpService)
        {
            config = iConfig;
            cache = iCache;
            httpService = iHttpService;
        }

        [HttpGet]
        public async Task<ChartResult> GetChartData(
            string symbol,
            string interval = "1d", // Default interval is 1 day
            string range = "5d",   // Default range is 5 days
            long? period1 = null,  // Default period1 is null (to be calculated)
            long? period2 = null)  // Default period2 is null (to be calculated)
        {
            // Construct the API URL
            var apiUrl = $"{symbol}?interval={interval}&range={range}&period1={period1}&period2={period2}";
            var cacheKey = symbol + interval + range;

            try
            {
                if (!cache.TryGetValue(cacheKey, out ChartResult chart))
                {
                    chart = await httpService.GetAsync<ChartResult>(apiUrl);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                    if (chart.StatusCode == (int)HttpStatusCode.OK)
                    {
                        // Cache the data for future requests
                        cache.Set(cacheKey, chart, cacheEntryOptions);
                    }
                    else
                    {
                        // Handle non-success status codes (e.g., log, throw exception, etc.)
                        throw new HttpRequestException($"HTTP request failed with status code {chart.StatusCode}");
                    }
                }

                return chart;
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching stock data: {ex.Message}");
            }
        }
    }
}
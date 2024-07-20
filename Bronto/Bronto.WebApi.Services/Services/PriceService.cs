using Bronto.Models.Api;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Bronto.WebApi.Services
{
    public class PriceService : IPriceService
    {
        private readonly IMemoryCache cache;
        private readonly ITwelveHttpService httpService;
        private IConfiguration config { get; set; }

        public PriceService(IConfiguration iConfig, IMemoryCache iCache, ITwelveHttpService iTwelveHttpService)
        {
            config = iConfig;
            cache = iCache;
            httpService = iTwelveHttpService;
        }

        public async Task<RealTimePrice> GetPriceData(string symbol)
        {            
            try
            {
                if (!cache.TryGetValue(symbol, out RealTimePrice price))
                {
                    price = await httpService.GetAsync<RealTimePrice>($"price?symbol={symbol}");

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                    if (price.StatusCode == (int)HttpStatusCode.OK)
                    {
                        // Cache the data for future requests
                        cache.Set(symbol, price, cacheEntryOptions);
                    }
                    else
                    {
                        // Handle non-success status codes (e.g., log, throw exception, etc.)
                        throw new HttpRequestException($"HTTP request failed with status code {price.StatusCode}");
                    }
                }

                return price;
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

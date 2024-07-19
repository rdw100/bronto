using Bronto.Models.Api;
using Bronto.WebApi.Services.Http;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

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
                    string apiUrl = $"price?symbol={symbol}";
                    price = await httpService.GetAsync<RealTimePrice>(apiUrl);
                    
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);

                    cache.Set(symbol, price, cacheEntryOptions);
                }

                return price;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching stock data: {ex.Message}");
            }
        }
    }
}

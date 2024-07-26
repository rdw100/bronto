using Bronto.Models.Api.Quote;
using Bronto.WebApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Bronto.WebApi.Services.Services
{
    public class QuoteService : IQuoteService
    {        
        private readonly IConfiguration config;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CookieCacheKey = "FreeApiCookie";
        private const string CrumbCacheKey = "FreeApiCrumb";
        private readonly string Cookie;
        private readonly string Crumb;
        private readonly string Quote;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="cache"></param>
        /// <remarks>Requests without headers receive 429 (Too Many Requests).</remarks>
        public QuoteService(IConfiguration iConfig, HttpClient httpClient, IMemoryCache cache)
        {
            config = iConfig;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            _cache = cache;
            Cookie = config.GetSection("FreeApi")["Cookie"];
            Crumb = config.GetSection("FreeApi")["Crumb"];
            Quote = config.GetSection("FreeApi")["Quote"];
        }

        public async Task<QuoteResult> GetQuote(string symbol)
        {
            if (!_cache.TryGetValue(CookieCacheKey, out string cookie) ||
                !_cache.TryGetValue(CrumbCacheKey, out string crumb))
            {
                cookie = await GetFreeApiCookieAsync();
                crumb = await GetFreeApiCrumbAsync(cookie);
                _cache.Set(CookieCacheKey, cookie);
                _cache.Set(CrumbCacheKey, crumb);
            }

            return await GetFreeApiQuoteAsync(symbol, cookie, crumb);
        }

        private async Task<string> GetFreeApiCookieAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Cookie);
            var response = await _httpClient.SendAsync(request);

            // Check if the status code is 404 matches
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound 
                || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                return setCookieHeader?.Split(';').FirstOrDefault();
            }

            throw new HttpRequestException("Unexpected response status code.");
        }

        private async Task<string> GetFreeApiCrumbAsync(string cookie)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Crumb);
            request.Headers.Add("Cookie", cookie);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<QuoteResult> GetFreeApiQuoteAsync(string symbol, string cookie, string crumb)
        {
            var url = $"{Quote}?symbols={symbol}&crumb={crumb}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", cookie);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<QuoteResult>();
        }
    }
}

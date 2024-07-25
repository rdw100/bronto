using Microsoft.Extensions.Caching.Memory;

namespace Bronto.WebApi.Services.Services
{
    public class QuoteService
    {        
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CookieCacheKey = "YahooFinanceCookie";
        private const string CrumbCacheKey = "YahooFinanceCrumb";

        public QuoteService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            _cache = cache;
        }

        public async Task<string> GetQuoteAsync(string symbol)
        {
            if (!_cache.TryGetValue(CookieCacheKey, out string cookie) ||
                !_cache.TryGetValue(CrumbCacheKey, out string crumb))
            {
                cookie = await GetYahooFinanceCookieAsync();
                crumb = await GetYahooFinanceCrumbAsync(cookie);
                _cache.Set(CookieCacheKey, cookie);
                _cache.Set(CrumbCacheKey, crumb);
            }

            return await GetYahooFinanceQuoteAsync(symbol, cookie, crumb);
        }

        private async Task<string> GetYahooFinanceCookieAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://fc.yahoo.com");
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

        private async Task<string> GetYahooFinanceCrumbAsync(string cookie)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://query2.finance.yahoo.com/v1/test/getcrumb");
            request.Headers.Add("Cookie", cookie);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetYahooFinanceQuoteAsync(string symbol, string cookie, string crumb)
        {
            var url = $"https://query2.finance.yahoo.com/v7/finance/quote?symbols={symbol}&crumb={crumb}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", cookie);
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

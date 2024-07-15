using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Bronto.WebApi.Services.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient HttpClient;
        private readonly string BaseUrl;

        public HttpService(HttpClient httpClient, IConfiguration config)
        {
            BaseUrl = config.GetSection("FreeApiV8")["Host"];
            httpClient.BaseAddress = new Uri($"https://{BaseUrl}/");
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            HttpClient = httpClient;
        }

        public Task<T> GetAsync<T>(string url)
        {
            return HttpClient.GetFromJsonAsync<T>(url);
        }
    }
}

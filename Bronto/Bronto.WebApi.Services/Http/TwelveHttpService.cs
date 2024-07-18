using Bronto.Models.Api;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Bronto.WebApi.Services.Http
{
    public class TwelveHttpService : IHttpService
    {
        private readonly HttpClient HttpClient;
        private readonly string BaseUrl;
        protected internal string Key { get; set; }
        protected internal string Host { get; set; }
        
        public TwelveHttpService(HttpClient httpClient, IConfiguration config)
        {
            Key = config.GetSection("AppSettings")["Key"];
            BaseUrl = config.GetSection("AppSettings")["Host"];
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri($"https://{BaseUrl}/");
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        public Task<T> GetAsync<T>(string url)
        {
            return HttpClient.GetFromJsonAsync<T>($"{url}&apikey={Key}");
        }
    }
}

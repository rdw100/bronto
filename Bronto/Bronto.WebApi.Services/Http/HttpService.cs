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
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri($"https://{BaseUrl}/");
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        public async Task<T> GetAsync<T>(string url) where T : new()
        {
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var result = new T();
            var statusCodeProperty = typeof(T).GetProperty("StatusCode");
            var statusMessageProperty = typeof(T).GetProperty("StatusMessage");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<T>();
                if (statusCodeProperty != null)
                {
                    statusCodeProperty.SetValue(content, 200);
                }

                if (statusMessageProperty != null)
                {
                    statusMessageProperty.SetValue(content, "OK");
                }

                return content;
            }
            else
            {
                if (statusCodeProperty != null)
                {
                    statusCodeProperty.SetValue(result, (int)response.StatusCode);
                }

                if (statusMessageProperty != null)
                {
                    statusMessageProperty.SetValue(result, response.ReasonPhrase);
                }

                return result;
            }
        }
    }
}

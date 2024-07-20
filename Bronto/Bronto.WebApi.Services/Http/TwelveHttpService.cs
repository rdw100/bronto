using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Bronto.WebApi.Services.Http
{
    public class TwelveHttpService : ITwelveHttpService
    {
        private readonly HttpClient HttpClient;
        protected internal string Key { get; set; }
        protected internal string Host { get; set; }
        
        public TwelveHttpService(HttpClient httpClient, IConfiguration config)
        {
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri($"https://{Host}/");
            HttpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        /// <summary>
        /// Accepts a generic type while handling unsuccessful responses gracefully
        /// </summary>
        /// <typeparam name="T">The generic type parameter T</typeparam>
        /// <param name="url">The api endpoint</param>
        /// <returns>If the response is successful, it deserializes the content to type T and returns it.  If the response is not successful, it creates a new instance of T and sets the StatusCode and StatusMessage properties if they exist.</returns>
        /// <remarks>Ensure that the type T has StatusCode and StatusMessage properties.</remarks>
        public async Task<T> GetAsync<T>(string url) where T : new()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"{url}&apikey={Key}");
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

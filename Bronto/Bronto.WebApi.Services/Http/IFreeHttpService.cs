namespace Bronto.WebApi.Services.Http
{
    public interface IFreeHttpService
    {
        Task<HttpResponseMessage> GetAsync(HttpRequestMessage request);
        Task<T> SendAsync<T>(HttpRequestMessage request) where T : new();
    }
}

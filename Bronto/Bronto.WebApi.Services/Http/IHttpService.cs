namespace Bronto.WebApi.Services.Http
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string url) where T : new();
    }
}

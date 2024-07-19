namespace Bronto.WebApi.Services.Http
{
    public interface ITwelveHttpService
    {
        Task<T> GetAsync<T>(string url);
    }
}

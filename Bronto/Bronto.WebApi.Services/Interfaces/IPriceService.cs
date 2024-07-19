using Bronto.Models.Api;

namespace Bronto.WebApi.Services.Interfaces
{
    public interface IPriceService
    {
        Task<RealTimePrice> GetPriceData(
            string symbol
        );
    }
}

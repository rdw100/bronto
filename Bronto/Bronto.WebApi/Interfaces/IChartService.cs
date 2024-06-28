using Bronto.Models;

namespace Bronto.WebApi.Interfaces
{
    public interface IChartService
    {
        Task<List<MyOHLC>> GetStockData(
                string symbol,
                string interval = "1d",
                string range = "5d",
                long? period1 = null,
                long? period2 = null
            ); 
    }
}

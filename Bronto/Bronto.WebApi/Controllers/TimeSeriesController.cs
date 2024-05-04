using Bronto.Models.Api;
using Bronto.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSeriesController : ControllerBase
    {
        private readonly TimeSeriesService _timeSeriesService;
        private readonly IConfiguration _config;
        protected internal string Key { get; set; }

        public TimeSeriesController(IConfiguration config)
        {
            _config = config;
            _timeSeriesService = new TimeSeriesService(_config);
        }

        [HttpGet]
        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval, string outputsize)
        {
            try
            {
                var timeSeriesData = await _timeSeriesService.GetTimeSeriesAsync(symbol, interval, outputsize);
                if (timeSeriesData != null)
                {
                    return timeSeriesData;
                }
                else
                {
                    return new StockDataTimeSeries()
                    {
                        ResponseStatus = Enums.StockDataClientResponseStatus.StockDataError,
                        ResponseMessage = "Error retrieving stock data"
                    };
                }                    
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
using Bronto.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bronto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSeriesController : ControllerBase
    {
        private readonly HttpClient _client;
        protected internal string Key { get; set; }
        protected internal string Host { get; set; }
        private IConfiguration config { get; set; }

        public TimeSeriesController(IConfiguration iConfig)
        {
            config = iConfig;
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"https://{Host}");
        }

        [HttpGet]
        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = $"https://{Host}/time_series?symbol={symbol}&interval={interval}&apikey={Key}";

                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<StockTimeSeries>(responseString);
                List<TimeSeriesValues> values = new List<TimeSeriesValues>();
                var tsStockValues = jsonResponse?.Values;
                if (tsStockValues != null)
                {
                    values.AddRange(tsStockValues.Select(v => new TimeSeriesValues()
                    {
                        Datetime = v?.Datetime ?? DateTime.MinValue,
                        Open = Convert.ToDouble(v?.Open),
                        High = Convert.ToDouble(v?.High),
                        Low = Convert.ToDouble(v?.Low),
                        Close = Convert.ToDouble(v?.Close),
                        Volume = v.Volume
                    }));
                }

                StockDataTimeSeries timeSeries = new StockDataTimeSeries()
                {
                    Symbol = jsonResponse?.Meta?.Symbol,
                    Interval = jsonResponse?.Meta?.Interval,
                    ExchangeTimezone = jsonResponse?.Meta?.ExchangeTimezone,
                    Exchange = jsonResponse?.Meta?.Exchange,
                    Type = jsonResponse?.Meta?.Type,
                    Currency = jsonResponse?.Meta?.Currency,
                    Values = values
                };

                if (string.IsNullOrEmpty(timeSeries?.Symbol) || values.Count == 0)
                {
                    timeSeries.ResponseStatus = Enums.StockDataClientResponseStatus.StockDataApiError;
                    timeSeries.ResponseMessage = "Invalid symbol or API key";

                    return timeSeries;
                }

                return timeSeries;
            }
            catch (Exception e)
            {
                return new StockDataTimeSeries()
                {
                    ResponseStatus = Enums.StockDataClientResponseStatus.StockDataError,
                    ResponseMessage = e.ToString()
                };
            }
        }
    }
}
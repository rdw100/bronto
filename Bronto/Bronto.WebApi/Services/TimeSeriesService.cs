using Bronto.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bronto.WebApi.Services
{
    public class TimeSeriesService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration config { get; set; }

        protected internal string Key { get; set; }
        protected internal string Host { get; set; }
        protected internal string uriString { get; set; }

        public TimeSeriesService(IConfiguration iConfig)
        {
            config = iConfig;
            Key = config.GetSection("AppSettings")["Key"];
            Host = config.GetSection("AppSettings")["Host"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{Host}");
        }

        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min")
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"time_series?symbol={symbol}&interval={interval}&apikey={Key}");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<StockTimeSeries>(responseString);
                    List<TimeSeriesValues> values = new List<TimeSeriesValues>();
                    var tsStockValues = jsonResponse?.Values;
                    if (tsStockValues != null)
                    {
                        values.AddRange(tsStockValues.Select(v => new TimeSeriesValues()
                        {
                            Datetime = v.Datetime, // v?.Datetime, ?? DateTime.MinValue,
                            Open = Convert.ToDouble(v?.Open),
                            High = Convert.ToDouble(v?.High),
                            Low = Convert.ToDouble(v?.Low),
                            Close = Convert.ToDouble(v?.Close),
                            Volume = v.Volume,
                            TimeSpan = TimeSpan.FromDays(1.0)
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
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new StockDataTimeSeries()
                {
                    ResponseStatus = Enums.StockDataClientResponseStatus.StockDataError,
                    ResponseMessage = ex.ToString()
                };
            }
        }
    }
}
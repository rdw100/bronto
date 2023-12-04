using Bronto.Tests.Api.Models;
using Newtonsoft.Json;

namespace Bronto.Tests.Api
{
    public class StockApiClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public StockApiClient(string key, HttpClient client)
        {
            _apiKey = key;
            _client = client;
        }

        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/time_series?symbol=" + symbol + "&interval=" + interval +
                                  "&apikey=" + _apiKey;

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

                if (!string.IsNullOrEmpty(timeSeries?.Symbol) && values.Count != 0)
                    return timeSeries;
                timeSeries.ResponseStatus = Enums.StockDataClientResponseStatus.StockDataApiError;
                timeSeries.ResponseMessage = "Invalid symbol or API key";

                return timeSeries;
            }
            catch (Exception e)
            {
                return new StockDataTimeSeries()
                {
                    ResponseStatus = Enums.StockDataClientResponseStatus.StockDataSharpError,
                    ResponseMessage = e.ToString()
                };
            }
        }
    }
}
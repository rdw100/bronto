using Bronto.Models.Api.Price.Response;
using Bronto.Tests.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Bronto.Models.Api.Enums;

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

        public async Task<RealTimePrice> GetRealTimePriceAsync(string symbol)
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/price?symbol=" + symbol + "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                RealTimePrice responsePrice = JsonConvert.DeserializeObject<RealTimePrice>(responseString);
                if (responsePrice.Price.Equals(0))
                {
                    responsePrice.ResponseStatus = StockDataClientResponseStatus.StockDataApiError;
                    responsePrice.ResponseMessage = "Invalid symbol or key.";
                    return responsePrice;                   
                }
                
                return responsePrice;
            }
            catch (Exception e)
            {
                return new RealTimePrice()
                {
                    ResponseStatus = StockDataClientResponseStatus.StockDataError,
                    ResponseMessage = e.ToString()
                };
            }
        }

        public async Task<StockSymbolSearch> GetStockSymbolAsync(string symbol)
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/symbol_search?symbol=" + symbol +
                    "&show_plan=true"+
                    "&apikey=" + _apiKey;
                var response = await _client.GetAsync(endpoint);
                string responseString = await response.Content.ReadAsStringAsync();
                StockSymbolSearch responseSymbol = JsonConvert.DeserializeObject<StockSymbolSearch>(responseString);

                if (string.IsNullOrEmpty(responseSymbol.Data.Symbol))
                {
                    responseSymbol.ResponseStatus = StockDataClientResponseStatus.StockDataApiError;
                    responseSymbol.ResponseMessage = "Invalid symbol or key.";
                    return responseSymbol;
                }
                
                return responseSymbol;
            }
            catch (Exception e)
            {
                return new StockSymbolSearch()
                {
                    ResponseStatus = StockDataClientResponseStatus.StockDataError,
                    ResponseMessage = e.ToString()
                };
            }
        }
        
        public async Task<StockDataTimeSeries> GetTimeSeriesAsync(string symbol, string interval = "1min", string outputsize = "30")
        {
            try
            {
                string endpoint = "https://api.twelvedata.com/time_series?"
                    + "symbol=" + symbol 
                    + "&interval=" + interval
                    + "&outputsize=" + outputsize
                    + "&apikey=" + _apiKey;

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
                    timeSeries.ResponseStatus = StockDataClientResponseStatus.StockDataApiError;
                    timeSeries.ResponseMessage = "Invalid symbol or API key";

                    return timeSeries;
                }

                return timeSeries;
            }
            catch (Exception e)
            {
                return new StockDataTimeSeries()
                {
                    ResponseStatus = StockDataClientResponseStatus.StockDataError,
                    ResponseMessage = e.ToString()
                };
            }
        }
    }
}
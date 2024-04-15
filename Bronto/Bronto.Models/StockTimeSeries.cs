using Newtonsoft.Json;

namespace Bronto.Models.Api
{
    public class StockTimeSeries
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("values")]
        public List<Value> Values { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("exchange_timezone")]
        public string ExchangeTimezone { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("close")]
        public string Close { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }
    }

    public class Enums
    {
        public enum StockDataClientResponseStatus
        {
            Ok,
            StockDataError,
            StockDataApiError
        }
    }

    public class StockDataTimeSeries : BaseResponse
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public string ExchangeTimezone { get; set; }
        public string Exchange { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public List<TimeSeriesValues> Values { get; set; }
    }

    public partial class TimeSeriesValues
    {
        public DateTime Datetime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }
}

using Bronto.Models.Api.Price.Response;
using Newtonsoft.Json;

namespace Bronto.Tests.Api
{
    public class StockSymbolSearch : BaseResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("mic_code")]
        public string MicCode { get; set; }

        [JsonProperty("instrument_type")]
        public string InstrumentType { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("access")]
        public Access Access { get; set; }
    }

    public partial class Access
    {
        [JsonProperty("global")]
        public string Global { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }
    }
}

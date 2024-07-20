using Newtonsoft.Json;
using static Bronto.Models.Api.Enums;

namespace Bronto.Models.Api
{
    public class RealTimePrice : ApiResponse
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }

    public class ApiResponse
    {
        public StockDataClientResponseStatus StatusCodeType { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = "RESPONSE_OK";
    }
}

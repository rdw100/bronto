using Newtonsoft.Json;

namespace Bronto.Models.Api
{
    public class RealTimePrice : BaseResponse
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }

    public class BaseResponse
    {
        public Enums.StockDataClientResponseStatus ResponseStatus { get; set; }
        public string ResponseMessage { get; set; } = "RESPONSE_OK";
    }
}

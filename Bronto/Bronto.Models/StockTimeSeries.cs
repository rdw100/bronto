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
            StockDataApiError,
            RateLimitExceeded
        }

        public enum HttpApiStatusCode
        {
            // Informational responses (100–199)
            Continue = 100,
            SwitchingProtocols = 101,
            Processing = 102,
            EarlyHints = 103,

            // Successful responses (200–299)
            Ok = 200,
            Created = 201,
            Accepted = 202,
            NonAuthoritativeInformation = 203,
            NoContent = 204,
            ResetContent = 205,
            PartialContent = 206,
            MultiStatus = 207,
            AlreadyReported = 208,
            IMUsed = 226,

            // Redirection messages (300–399)
            MultipleChoices = 300,
            MovedPermanently = 301,
            Found = 302,
            SeeOther = 303,
            NotModified = 304,
            UseProxy = 305,
            TemporaryRedirect = 307,
            PermanentRedirect = 308,

            // Client error responses (400–499)
            BadRequest = 400,
            Unauthorized = 401,
            PaymentRequired = 402,
            Forbidden = 403,
            NotFound = 404,
            MethodNotAllowed = 405,
            NotAcceptable = 406,
            ProxyAuthenticationRequired = 407,
            RequestTimeout = 408,
            Conflict = 409,
            Gone = 410,
            LengthRequired = 411,
            PreconditionFailed = 412,
            PayloadTooLarge = 413,
            URITooLong = 414,
            UnsupportedMediaType = 415,
            RangeNotSatisfiable = 416,
            ExpectationFailed = 417,
            ImATeapot = 418,
            MisdirectedRequest = 421,
            UnprocessableEntity = 422,
            Locked = 423,
            FailedDependency = 424,
            TooEarly = 425,
            UpgradeRequired = 426,
            PreconditionRequired = 428,
            TooManyRequests = 429,
            RequestHeaderFieldsTooLarge = 431,
            UnavailableForLegalReasons = 451,

            // Server error responses (500–599)
            InternalServerError = 500,
            NotImplemented = 501,
            BadGateway = 502,
            ServiceUnavailable = 503,
            GatewayTimeout = 504,
            HTTPVersionNotSupported = 505,
            VariantAlsoNegotiates = 506,
            InsufficientStorage = 507,
            LoopDetected = 508,
            NotExtended = 510,
            NetworkAuthenticationRequired = 511
        }

    }

    public class StockDataTimeSeries : ApiResponse
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
        public TimeSpan TimeSpan { get; set; }
    }
}

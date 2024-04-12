namespace Bronto.Stocks.Console
{
    public class TimeSeries
    {
        public Dictionary<string, string> meta { get; set; }
        public IList<Dictionary<string, string>> values { get; set; }
        public string status { get; set; }
    }
}
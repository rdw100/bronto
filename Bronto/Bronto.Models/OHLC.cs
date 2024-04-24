namespace Bronto.Models
{
    public struct MyOHLC
    {
        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public DateTime DateTime { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public MyOHLC(double open, double high, double low, double close, DateTime start, TimeSpan span)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = start;
            TimeSpan = span;
        }

        public static List<MyOHLC> GenerateOHLCs(int number)
        {
            List<MyOHLC> ohlcs = new List<MyOHLC>();
            Random rand = new Random();
            DateTime start = DateTime.Now;
            TimeSpan span = TimeSpan.FromMinutes(1);
            for (int i = 0; i < number; i++)
            {
                double open = rand.NextDouble() * 100;
                double high = open + rand.NextDouble() * 10;
                double low = open - rand.NextDouble() * 10;
                double close = low + rand.NextDouble() * (high - low);
                ohlcs.Add(new MyOHLC(open, high, low, close, start, span));
                start += span;
            }
            return ohlcs;
        }   
    }
}
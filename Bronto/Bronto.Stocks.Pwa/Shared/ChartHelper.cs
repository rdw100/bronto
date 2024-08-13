using ScottPlot;
using ScottPlot.Finance;

public static class ChartHelper
{
    public static async Task<Plot> PlotSmaCurves(Plot plot, List<OHLC> prices)
    {
        int[] windowSizes = { 3, 8, 20 };
        foreach (int windowSize in windowSizes)
        {
            // Calculate SMA
            SimpleMovingAverage sma = new(prices, windowSize);

            // Plot the SMA on the provided BlazorPlot
            var sp = plot.Add.Scatter(sma.Dates, sma.Means);
            sp.LegendText = $"SMA {windowSize}";
            sp.MarkerSize = 0;
            sp.LineWidth = 3;
            sp.Color = Colors.Navy.WithAlpha(1 - windowSize / 30.0);
        }

        return plot;
    }

    public static async Task<Plot> PlotBollinger(Plot plot, List<OHLC> prices)
    {
        // calculate Bollinger Bands
        BollingerBands bb = new(prices, 20);

        // display center line (mean) as a solid line
        var sp1 = plot.Add.Scatter(bb.Dates, bb.Means);
        sp1.MarkerSize = 0;
        sp1.Color = Colors.Navy;

        // display upper bands (positive variance) as a dashed line
        var sp2 = plot.Add.Scatter(bb.Dates, bb.UpperValues);
        sp2.MarkerSize = 0;
        sp2.Color = Colors.Navy;
        sp2.LinePattern = LinePattern.Dotted;

        // display lower bands (positive variance) as a dashed line
        var sp3 = plot.Add.Scatter(bb.Dates, bb.LowerValues);
        sp3.MarkerSize = 0;
        sp3.Color = Colors.Navy;
        sp3.LinePattern = LinePattern.Dotted;
        
        return plot;
    }
}

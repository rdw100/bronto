using BlazorPro.BlazorSize;
using Bronto.Stocks.Pwa;
using Bronto.Stocks.Pwa.Interfaces;
using Bronto.Stocks.Pwa.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMediaQueryService();
builder.Services.AddScoped<IResizeListener, ResizeListener>();

builder.Services.AddHttpClient<IStockService, StockService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});
builder.Services.AddHttpClient<IPriceService, PriceService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});
builder.Services.AddHttpClient<IQuoteService, QuoteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});
builder.Services.AddHttpClient<IChartService, ChartService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});
builder.Services.AddSingleton<IWatchlistService, WatchlistService>();
builder.Services.AddSingleton<IPortfolioService, PortfolioService>();

builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
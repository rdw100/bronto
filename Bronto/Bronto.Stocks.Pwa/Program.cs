using Bronto.Stocks.Pwa;
using Bronto.Stocks.Pwa.Interfaces;
using Bronto.Stocks.Pwa.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IStockService, StockService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
});
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
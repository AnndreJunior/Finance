using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Finance.Web;
using MudBlazor.Services;
using Finance.Core;
using Finance.Core.Handlers;
using Finance.Web.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddHttpClient(WebConfiguration.HttpClientName, opts =>
{
    opts.BaseAddress = new Uri(Configuration.BackedUrl);
});

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

await builder.Build().RunAsync();

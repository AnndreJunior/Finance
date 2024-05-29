using Finance.Api;
using Finance.Api.Common.Api;
using Finance.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDataContext();
builder.AddCorsOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfiguration.CorsPolicyName);

// app.UseSecurity();

app.MapEndpoints();

app.Run();

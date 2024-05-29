using Finance.Api.Data;
using Finance.Api.Handlers;
using Finance.Core;
using Finance.Core.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Finance.Api.Common.Api;

public static class BuilderExtention
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DbConnection") ?? string.Empty;
        Configuration.BackedUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Finance Api"
            });
        });
    }

    public static void AddDataContext(this WebApplicationBuilder builder)
        => builder.Services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(ApiConfiguration.ConnectionString));

    public static void AddCorsOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(opts => opts.AddPolicy(
            ApiConfiguration.CorsPolicyName,
            policy => policy.WithOrigins([
                Configuration.BackedUrl,
                Configuration.FrontendUrl
            ])
            .AllowAnyHeader()
            .AllowAnyHeader()
            .AllowCredentials()
        ));
    }


    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
    }

}

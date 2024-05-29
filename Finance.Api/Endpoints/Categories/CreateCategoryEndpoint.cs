using Finance.Api.Common.Api;
using Finance.Core.Handlers;
using Finance.Core.Models;
using Finance.Core.Requests.Categories;
using Finance.Core.Responses;

namespace Finance.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("", HandleAsync)
        .WithName("CreateCategory")
        .WithSummary("Create a category")
        .WithOrder(1)
        .Produces<Response<Category?>>(StatusCodes.Status201Created)
        .Produces<Response<Category?>>(StatusCodes.Status400BadRequest);

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler,
        CreateCategoryRequest request)
    {
        request.Userid = ApiConfiguration.UserId; // UserId mock value
        var response = await handler.CreateAsync(request);

        return response.IsSuccess
            ? TypedResults.Created($"v1/categories/{response.Data?.Id}", response)
            : TypedResults.BadRequest(response);
    }
}

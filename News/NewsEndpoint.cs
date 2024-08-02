using Microsoft.AspNetCore.Http.HttpResults;

namespace MockAPI.News;

public static class NewsEndpoint
{
    public static void RegisterNewsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var groupBuilder = endpointRouteBuilder.MapGroup("api/news/")
            .WithTags("News")
            .WithDescription("Get the latest news")
            .WithOpenApi();

        groupBuilder.MapGet("retrieveNews", RetrieveNewsAsync)
            .Produces<List<News>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all news")
            .WithDescription("\n    GET api/news/retrieveNews");
    }

    public static async Task<IResult> RetrieveNewsAsync(INewsService newsService)
    {
        var result = await newsService.RetrieveNews();
        
        return Results.Ok(result);
    }
}

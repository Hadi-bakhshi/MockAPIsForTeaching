namespace MockAPI.Endpoints;

public static class NewsEndpoint
{
    public static void RegisterNewsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var groupBuilder = endpointRouteBuilder.MapGroup("api/news/")
            .WithTags("News")
            .WithDescription("Get the latest news")
            .WithOpenApi();

        groupBuilder.MapGet("retrieveNews", RetrieveNews)
            .Produces<string>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all news")
            .WithDescription("\n    GET api/news/retrieveNews");
    }

    public static IResult RetrieveNews()
    {
        try
        {
            return Results.Ok("Hello from me");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}

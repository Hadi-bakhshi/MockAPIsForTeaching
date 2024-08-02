using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MockAPI.News.Contracts;

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


        groupBuilder.MapPost("createNews", CreateNewsAsync)
            .Produces<News>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create news")
            .WithDescription("\n    POST api/news/createNews");

        groupBuilder.MapGet("retrieveNewsById/{id:guid}", RetrieveNewsByIdAsync)
            .Produces<News>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get a specific news")
            .WithDescription("\n    GET api/news/retrieveNewsById");

        groupBuilder.MapPut("updateNews", UpdateNewsAsync)
            .Produces<News>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update news")
            .WithDescription("\n    PUT api/news/updateNews");

        groupBuilder.MapDelete("removeNewsById/{id:guid}", RemoveNewsByIdAsync)
           .ProducesValidationProblem()
           .ProducesProblem(StatusCodes.Status500InternalServerError)
           .WithSummary("Remove a specific news")
           .WithDescription("\n    DELETE api/news/removeNewsById");
    }

    public static async Task<IResult> RetrieveNewsAsync(INewsService newsService)
    {
        var result = await newsService.RetrieveNews();

        return Results.Ok(result);
    }
    public static async Task<IResult> CreateNewsAsync([FromBody] CreateNewsRequest createNewsRequest, INewsService newsService)
    {
        var mappedRequestToServiceParameter = createNewsRequest.Adapt<News>();
        var result = await newsService.CreateNewsAsync(mappedRequestToServiceParameter);
        return Results.Created(nameof(CreateNewsAsync), result);
    }
    public static async Task<IResult> RetrieveNewsByIdAsync([FromRoute] Guid id, INewsService newsService)
    {
        var result = await newsService.GetNewsByIdAsync(id);
        return Results.Ok(result);
    }
    public static async Task<IResult> UpdateNewsAsync([FromBody] UpdateNewsRequest updateNewsRequest, INewsService newsService)
    {
        var mappedRequestToServiceParameter = updateNewsRequest.Adapt<News>();
        var result = await newsService.UpdateNewsAsync(mappedRequestToServiceParameter);
        return Results.Ok(result);
    }

    public static async Task<IResult> RemoveNewsByIdAsync([FromRoute] Guid id, INewsService newsService)
    {
        await newsService.DeleteNewsAsync(id);
        return Results.Ok("Deleted");
    }

}

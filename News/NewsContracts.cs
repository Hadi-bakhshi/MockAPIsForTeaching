using System.ComponentModel;

namespace MockAPI.News.Contracts;

public class CreateNewsRequest
{
    [DisplayName("Title")]
    public required string Title { get; set; }

    [DisplayName("Description")]
    public required string Description { get; set; }

    [DisplayName("Category")]
    public required string Category { get; set; }

    [DisplayName("Url")]
    public required string Url { get; set; }
}
public class UpdateNewsRequest
{
    [DisplayName("Title")]
    public Guid Id { get; set; }
    [DisplayName("Title")]
    public string? Title { get; set; }

    [DisplayName("Description")]
    public string? Description { get; set; }

    [DisplayName("Category")]
    public string? Category { get; set; }

    [DisplayName("Url")]
    public string? Url { get; set; }
}

using Newtonsoft.Json;
using System.Text.Json;

namespace MockAPI.News;

public class NewsService : INewsService
{
    private ILogger<NewsService> _logger;

    private const string DataDirectory = "LocalData";
    private const string DataFileName = "news.json";
    private const string DataFileDirectory = $"{DataDirectory}/{DataFileName}";

    public NewsService(ILogger<NewsService> logger)
    {
        _logger = logger;
    }

    public string GetFullPath()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(currentDir, DataFileDirectory);

        return fullPath;
    }

    public async Task<int> CreateJsonFileIfNotExist()
    {
        var fullPath = GetFullPath();

        if (!File.Exists(fullPath))
        {
            _logger.LogInformation("file doesn't exist, creating new one...");
            await using var fileStream = File.Create(fullPath);
            await fileStream.DisposeAsync();

            return 1;
        }

        return 0;

    }

    public async Task<List<News>> RetrieveNews()
    {

        var fullPath = GetFullPath();

        var fileCreationStatus = await CreateJsonFileIfNotExist();
        if (fileCreationStatus == 1)
        {
            return [];
        }
        // Read the file and get the content as text
        var fileContent = await File.ReadAllTextAsync(fullPath);

        // When there is no content then return empty list
        if (string.IsNullOrWhiteSpace(fileContent))
        {
            return [];
        }

        // Convert the json text to news list 
        var news = JsonConvert.DeserializeObject<List<News>>(fileContent);
        if (news == null || news.Count == 0)
        {
            return [];
        }

        return news;

    }

    public async Task<News> CreateNewsAsync(News news)
    {
        news.Id = Guid.NewGuid();
        var allNews = await RetrieveNews();
        allNews.Add(news);

        var utf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(allNews);

        await File.WriteAllBytesAsync(GetFullPath(), utf8Bytes);

        return news;
    }

    public async Task<News> GetNewsByIdAsync(Guid id)
    {
        var allNews = await RetrieveNews();

        var foundNews = allNews
            .Where(news => news.Id == id)
            .FirstOrDefault();

        if (foundNews is null)
        {
            throw new BadHttpRequestException("Passed Id wasn't found");
        }

        return foundNews;
    }

    public async Task<News> UpdateNewsAsync(News news)
    {
        var allNews = await RetrieveNews();

        var foundNews = allNews
            .Where(n => n.Id == news.Id)
            .FirstOrDefault();

        if (foundNews is null)
        {
            throw new BadHttpRequestException("Passed Id wasn't found");
        }

        if (foundNews is not null)
        {
            foundNews.Title = news.Title == null ? string.Empty : news.Title;
            foundNews.Description = news.Description == null ? string.Empty : news.Description;
            foundNews.Category = news.Category == null ? string.Empty : news.Category;
            foundNews.Url = news.Url == null ? string.Empty : news.Url;
        }

        var utf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(allNews);

        await File.WriteAllBytesAsync(GetFullPath(), utf8Bytes);

        return foundNews;
    }

    public async Task DeleteNewsAsync(Guid id)
    {
        var allNews = await RetrieveNews();

        var foundNews = allNews.Where(n => n.Id == id).FirstOrDefault();
        if (foundNews is null)
        {
            throw new BadHttpRequestException("Passed Id wasn't found");
        }
        if (foundNews is not null)
        {
            allNews.Remove(foundNews);
        }
        var utf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(allNews);

        await File.WriteAllBytesAsync(GetFullPath(), utf8Bytes);
    }
}

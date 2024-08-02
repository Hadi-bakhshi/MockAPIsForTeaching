
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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


}

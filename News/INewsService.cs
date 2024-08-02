namespace MockAPI.News;

public interface INewsService
{
    Task<List<News>> RetrieveNews();

    /// <summary>
    /// Check if the Data directory and file exist. Create if they do not exist. Otherwise do nothing
    /// </summary>
    /// <returns>
    /// 1 when creates the file and 0 when did nothing
    /// </returns>
    Task<int> CreateJsonFileIfNotExist();
    /// <summary>
    /// This method will get the current working directory and then combine the defined directory with the current
    /// </summary>
    /// <returns>full path</returns>
    string GetFullPath();

    Task<News> CreateNewsAsync(News news);

    Task<News> GetNewsByIdAsync(Guid id);

    Task<News> UpdateNewsAsync(News news);

    Task DeleteNewsAsync(Guid id);
}

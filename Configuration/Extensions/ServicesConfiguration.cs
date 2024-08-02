using MockAPI.News;

namespace MockAPI.Configuration.Extensions;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServicesConfigurations(this IServiceCollection services)
    {
        services.AddScoped<INewsService, NewsService>();

        return services;
    }
}

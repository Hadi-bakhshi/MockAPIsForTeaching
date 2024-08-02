using Mapster;
using MapsterMapper;
using MockAPI.Infrastructure;
using System.Reflection;

namespace MockAPI.Configuration.Extensions;

public static class ApplicationConfiguration
{
    public static WebApplicationBuilder AddApplicationBuilderConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "cors",
                              policy =>
                              {
                                  policy.AllowAnyOrigin();
                                  policy.AllowAnyHeader();
                                  policy.AllowAnyMethod();
                              });
        });
        // Add Mapster 
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, ServiceMapper>();

        builder.Services.AddServicesConfigurations();
        return builder;
    }
}

using WebApplication1.Infrastructure;

namespace WebApplication1.Configuration.Extensions;

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
        return builder;
    }
}

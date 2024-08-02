using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MockAPI.Configuration.Extensions;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Simple CRUD for TQ presentation", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
        });
        return services;
    }
}

public class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
{
    const string CaptureName = "routeParameter";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethodAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>();

        var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template?.Contains('?') ?? false);
        if (httpMethodWithOptional == null)
            return;

        Console.WriteLine(httpMethodWithOptional);

        string regex = $"{{(?<{CaptureName}>\\w+)\\?}}";

        var matches = System.Text.RegularExpressions.Regex.Matches(httpMethodWithOptional.Template, regex);

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var name = match.Groups[CaptureName].Value;

            var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
            if (parameter != null)
            {
                parameter.AllowEmptyValue = true;
                parameter.Description = "Must check \"Send empty value\" or Swagger passes a comma for empty values otherwise";
                parameter.Required = false;
                //parameter.Schema.Default = new OpenApiString(string.Empty);
                parameter.Schema.Nullable = true;
            }
        }
    }
}

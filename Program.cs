using MockAPI.Configuration.Extensions;
using MockAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.AddApplicationBuilderConfiguration();

var app = builder.Build();

app.UseStatusCodePages();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();


//app.UseAuthentication();

app.UseCors("cors");

//app.UseAuthorization();

app.RegisterNewsEndpoints();

app.MapHealthChecks("/health").RequireRateLimiting("fixed");

app.Run();


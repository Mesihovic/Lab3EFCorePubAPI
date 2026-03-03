using Microsoft.EntityFrameworkCore;
using PubDBAPI;          
using PublisherData;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI + Swagger
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(o =>
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);





// DbContext
builder.Services.AddDbContext<PubContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PubConnection"))
       .EnableSensitiveDataLogging()
       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// JSON cycles
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");



app.MapBookEndpoints();


app.MapControllers();

app.Run();



public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



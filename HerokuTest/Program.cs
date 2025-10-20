using HerokuTest.Commands;
using HerokuTest.Services;
using Newtonsoft.Json;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;


services.AddSingleton<TelegramBot>();
services.AddSingleton<ICommandExecutor, CommandExecutor>();
services.AddSingleton<BaseCommand, StartCommand>();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();



var app = builder.Build();

var serviceProvider =  app.Services;
serviceProvider.GetRequiredService<TelegramBot>().GetBot().Wait();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapGet("/", () => "Hello World!");
app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
app.MapPost("/api/message/update", async (HttpRequest request, ICommandExecutor commandExecutor) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    Update? update;

    try
    {
        update = JsonConvert.DeserializeObject<Update>(body);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка JSON: {ex.Message}");
        return Results.BadRequest();
    }

    if (update != null)
    {
        try
        {
            await commandExecutor.Execute(update);
        }
        catch (Exception e)
        {
            return Results.Ok();
        }
    }

    return Results.Ok();
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
using HerokuTest;
using HerokuTest.Commands;
using HerokuTest.Services;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddDbContext<DataContext>(options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));

services.AddSingleton<TelegramBot>();
services.AddScoped<ICommandExecutor, CommandExecutor>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ITrackingService, TrackingService>();
services.AddScoped<BaseCommand, StartCommand>();
services.AddScoped<BaseCommand, AdminCommand>();
services.AddScoped<BaseCommand, AddAdminCommand>();
services.AddScoped<BaseCommand, AddAdminExecuteCommand>();
services.AddScoped<BaseCommand, AddFirstAdminCommand>();
services.AddScoped<BaseCommand, AddTrackingCodeCommand>();
services.AddScoped<BaseCommand, AddFileProcessCommand>();
services.AddScoped<BaseCommand, GetContactsCommand>();
services.AddScoped<BaseCommand, GetProhibitedGoodsCommand>();
services.AddScoped<BaseCommand, GetPriceDeliveryCommand>();
services.AddScoped<BaseCommand, GetMarketAppsCommand>();
services.AddScoped<BaseCommand, GetTrackingCodeCommand>();
services.AddScoped<BaseCommand, FindByTrackingCodeCommand>();
services.AddScoped<BaseCommand, AddReceivedTrackingCodeCommand>();
services.AddScoped<BaseCommand, AddReceivedFileProcessCommand>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    try
    {
        Console.WriteLine("🚀 Применение миграций к базе данных...");
        db.Database.Migrate();
        Console.WriteLine("✅ Миграции успешно применены.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка при применении миграций: {ex.Message}");
    }
}


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
app.MapGet("/", () => "Hello World_0002!");
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
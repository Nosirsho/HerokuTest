using HerokuTest;
using HerokuTest.Commands;
using HerokuTest.Services;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

//services.AddDbContext<DataContext>(options => options.UseSqlite(config.GetConnectionString("DefaultConnection")));
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Преобразуем URL в стандартный формат
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};" +
                       $"Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
} else {
    // Локальное подключение (для разработки)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

services.AddDbContext<DataContext>(o => o.UseNpgsql(connectionString));
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
services.AddScoped<BaseCommand, GetGenerateAddressCommand>();
services.AddScoped<BaseCommand, AddAddressNameCommand>();
services.AddScoped<BaseCommand, AddAddressNumberCommand>();

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


app.MapGet("/", () => "Попытка_0004!");
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
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HerokuTest.Commands;

public abstract class BaseCommand
{
    public abstract string Name { get; }
    public abstract Task ExecuteAsync(Update update);

    public static async Task SendAsync(string[] trackcodes, DataContext context)
    {
        
    }
}
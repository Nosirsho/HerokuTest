using Telegram.Bot.Types;

namespace HerokuTest.Services
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class GetTrackingCodeCommand: BaseCommand
{
    private readonly TelegramBotClient _botClient;
    public GetTrackingCodeCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.GetTrackingCodeCommand;
    public override async Task ExecuteAsync(Update update)
    {
        const string message = "Введите трек-код!";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
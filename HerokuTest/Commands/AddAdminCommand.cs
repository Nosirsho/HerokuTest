using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class AddAdminCommand: BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;
    public AddAdminCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.AddAdminCommand;
    public override async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetOrCreate(update);
        if (!user.IsAdmin)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Error: You are not an admin!");
            return;
        }
        const string message = "Введите UserName|code";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
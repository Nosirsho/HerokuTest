using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class AddFirstAdminCommand: BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;
    public AddFirstAdminCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.AddFirstAdminCommand;
    public override async Task ExecuteAsync(Update update)
    {
        var userAdmin = await _userService.HasAdmin();
        if (userAdmin != null)
        {
            await _botClient.SendTextMessageAsync(userAdmin.ChatId, "Error: You are not an admin!");
            return;
        }
        var appUser = await _userService.GetOrCreate(update);
        appUser = await _userService.AddAdmin(appUser);
        const string message = "Hello";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
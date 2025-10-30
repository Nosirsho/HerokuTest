using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class AddAdminExecuteCommand: BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;
    public AddAdminExecuteCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.AddAdminExecuteCommand;
    public override async Task ExecuteAsync(Update update)
    {
        var message = "";
        var user = await _userService.GetOrCreate(update);
        if (!user.IsAdmin)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Error: You are not an admin!");
            return;
        }
        var username = update.Message.Text.Trim() == null ? string.Empty : update.Message.Text.Trim();
        var appUser = await _userService.GetUserByUsername(username);
        if (appUser != null)
        {
            appUser = await _userService.AddAdmin(appUser);
            message = $"Добавлен админ {username}";
        }
        else
        {
            message = $"не найден пользователь {username}";
        }

        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
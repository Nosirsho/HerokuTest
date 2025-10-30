using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HerokuTest.Commands;

public class AdminCommand : BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;

    public AdminCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.AdminCommand;

    public override async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetOrCreate(update);
        if (!user.IsAdmin)
        {
            await _botClient.SendTextMessageAsync(user.ChatId, "Error: You are not an admin!");
            return;
        }

        var inlineKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Добавление трек-кода") },
            new KeyboardButton[] { new KeyboardButton("Доб. полученные трек-коды") },
            new KeyboardButton[] { new KeyboardButton("Доб. админ") },
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(user.ChatId, "Выберите интересующий вас пункт меню!",
            replyMarkup:inlineKeyboard);
    }
}
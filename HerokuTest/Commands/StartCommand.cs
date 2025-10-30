using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HerokuTest.Commands;

public class StartCommand : BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;

    public StartCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.StartCommand;

    public override async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetOrCreate(update);
        var inlineKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Расценки и сроки доставки"), new KeyboardButton("Запрещенные товары") },
            new KeyboardButton[] { new KeyboardButton("Программы для установки"), new KeyboardButton("Контакты") },
            new KeyboardButton[] { new KeyboardButton("Поиск по трек-коду") }
        })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(user.ChatId, "Добро пожаловать! Для получение информации выберите интересующий вас пункт меню!",
            replyMarkup:inlineKeyboard);
    }
}
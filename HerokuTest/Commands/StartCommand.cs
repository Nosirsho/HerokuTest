using HerokuTest.Commands;
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HerokuTest.Commands;

public class StartCommand : BaseCommand
{
    private readonly TelegramBotClient _botClient;

    public StartCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.StartCommand;

    public override async Task ExecuteAsync(Update update)
    {
        var inlineKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { new KeyboardButton("Расценки и сроки доставки"), new KeyboardButton("Запрещенные товары") },
            new KeyboardButton[] { new KeyboardButton("Программы для установки"), new KeyboardButton("Контакты") },
            new KeyboardButton[] { new KeyboardButton("Поиск по трек-коду") },
            new KeyboardButton[] { new KeyboardButton("Кнопка 1"), new KeyboardButton("Кнопка 2"), new KeyboardButton("Кнопка 3") }
        })
        {
            ResizeKeyboard = true
        };
        var chatId =  update.Message.Chat.Id;

        await _botClient.SendTextMessageAsync(chatId, "Добро пожаловать! Для получение информации выберите интересующий вас пункт меню!",
            replyMarkup:inlineKeyboard);
    }
}
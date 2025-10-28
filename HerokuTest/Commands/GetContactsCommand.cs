using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class GetContactsCommand : BaseCommand
{
    private readonly TelegramBotClient _botClient;
    public GetContactsCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.GetContactsCommand;
    public override async Task ExecuteAsync(Update update)
    {
        const string message = "Наш адрес в Худжанде \nТрасса Худжанд-Гафуров \nРынок Атуш\n" +
                               "Контакты: \n" +
                               "Телефон - (+992) 11 900 13 10 \n" +
                               "Instagram - @chudo.tovar.tajikistan \n" +
                               "\u23ec АДРЕС СКЛАДА В КИТАЕ \u23ec \n" +
                               "ArzonCargo-tj\n19972639805\n\n" +
                               "浙江省义乌市福田街道 湖塘通福5区 21栋3单元123仓库(A192KZ)\n" +
                               "Имя + номер телефона";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
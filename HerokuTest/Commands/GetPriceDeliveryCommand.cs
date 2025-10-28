using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using HerokuTest.Services;

namespace HerokuTest.Commands;

public class GetPriceDeliveryCommand : BaseCommand
{
    private readonly TelegramBotClient _botClient;
    public GetPriceDeliveryCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.GetPriceDeliveryCommand;
    public override async Task ExecuteAsync(Update update)
    {
        const string message = "Ассалом алейкум \n\n" +
                               "НАШИ ЦЕНЫ:\n"+
                               "1 кг - 2.6$ с бесплатной доставкой до дома \n"+
                               "(ХУДЖАНД-ГАФУРОВ-БУСТОН)\n"+
                               "1 кг - 2.5$ самовывоз со склада на рынке Атуш \n\n"+
                               "СРОКИ ДОСТАВКИ: \n"+
                               "14-18 дней с момента поступления на склад в Китае";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HerokuTest.Commands;

public class GetMarketAppsCommand : BaseCommand
{
    private readonly TelegramBotClient _botClient;
    public GetMarketAppsCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.GetMarketAppsCommand;
    public override async Task ExecuteAsync(Update update)
    {
        using (var stream = System.IO.File.OpenRead("./Images/MarketApps.jpg"))
        {
            await _botClient.SendPhotoAsync(
                chatId: update.Message.Chat.Id,
                photo: new InputFileStream(stream, System.IO.Path.GetFileName("./Images/MarketApps.jpg")),
                caption: "Китайские маркетплейсы.\n\n" +
                         "<b>Pinduoduo</b> – <a href=\"https://pinduoduo.ru.malavida.com/android/download\">Android</a> | " +
                         "<a href=\"https://t.me/+6wmOpxRodI5kZDIy\">iOS</a>\n" +

                         "<b>TaoBao</b> – <a href=\"https://taobao.ru.uptodown.com/android\">Android</a> | " +
                         "<a href=\"https://apps.apple.com/by/app/taobao-online-shopping-app/id387682726\">iOS</a>\n" +

                         "<b>Poizon</b> – <a href=\"https://www.dewu.com/\">Android</a> | " +
                         "<a href=\"https://www.dewu.com/\">iOS</a>\n" +

                         "<b>95</b> – <a href=\"https://www.pgyer.com/apk/ru/apk/com.jiuwu\">Android</a> | " +
                         "<a href=\"https://apps.apple.com/by/app/95%E5%88%86-%E6%AD%A3%E5%93%81%E9%97%B2%E7%BD%AE%E4%BA%A4%E6%98%93%E5%B9%B3%E5%8F%B0/id1488709429\">iOS</a>\n" +

                         "<b>1688</b> – <a href=\"https://play.google.com/store/apps/details?id=com.alibaba.wireless&hl=ru\">Android</a> | " +
                         "<a href=\"https://apps.apple.com/by/app/1688-b2b-market/id507097717\">iOS</a>",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
            );
        }

    }
}
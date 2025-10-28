using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HerokuTest.Commands;

public class GetProhibitedGoodsCommand : BaseCommand
{
    private readonly TelegramBotClient _botClient;
    public GetProhibitedGoodsCommand(TelegramBot telegramBot)
    {
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.GetProhibitedGoodsCommand;
    public override async Task ExecuteAsync(Update update)
    {
        using (var stream = System.IO.File.OpenRead("./Images/Prohibited_goods.jpg"))
        {
            await _botClient.SendPhotoAsync(
                chatId: update.Message.Chat.Id,
                photo: new InputFileStream(stream, System.IO.Path.GetFileName("./Images/Prohibited_goods.jpg"))
                //caption: "Пример фотографии" // Необязательная подпись
            );
        }

    }
}
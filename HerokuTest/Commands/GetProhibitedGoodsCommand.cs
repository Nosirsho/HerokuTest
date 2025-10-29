using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

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
        var imagePath = Path.Combine(AppContext.BaseDirectory, "Images", "Prohibited_goods.jpg");

        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"❌ Файл не найден: {imagePath}");
            return;
        }

        using (var stream = File.OpenRead(imagePath))
        {
            await _botClient.SendPhotoAsync(
                chatId: update.Message.Chat.Id,
                photo: new InputFileStream(stream, Path.GetFileName(imagePath))
            );
        }

    }
}
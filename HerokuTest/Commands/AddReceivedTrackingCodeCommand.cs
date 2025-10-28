using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using HerokuTest.Services;

namespace HerokuTest.Commands
{
    public class AddReceivedTrackingCodeCommand: BaseCommand
    {
        private readonly TelegramBotClient _botClient;

        public AddReceivedTrackingCodeCommand(TelegramBot telegramBot)
        {
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.AddReceivedTrackingCodeCommand;
        
        public override async Task ExecuteAsync(Update update)
        {
            const string message = "ПОЛУЧЕННЫЕ  ТОВАРЫ!\nФайл со списком трек-кодов полученных заказчиками\nПрикрепите и отправьте файл \n Формат файла .txt";

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
        }
    }
}
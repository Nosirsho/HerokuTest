using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using HerokuTest.Services;

namespace HerokuTest.Commands
{
    public class AddTrackingCodeCommand: BaseCommand
    {
        private readonly TelegramBotClient _botClient;

        public AddTrackingCodeCommand(TelegramBot telegramBot)
        {
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.AddTrackingCodeCommand;
        
        public override async Task ExecuteAsync(Update update)
        {
            const string message = "Прирепите и отправьте файл \n Формат файла .txt";

            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
        }
    }
}
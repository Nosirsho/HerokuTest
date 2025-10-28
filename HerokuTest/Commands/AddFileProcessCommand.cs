using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using HerokuTest.Services;
using File = System.IO.File;

namespace HerokuTest.Commands
{
    public class AddFileProcessCommand: BaseCommand
    {
        private readonly ITrackingService _trackingService;
        private readonly TelegramBotClient _botClient;

        public AddFileProcessCommand(TelegramBot telegramBot, ITrackingService trackingService)
        {
            _trackingService = trackingService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.AddFileProcessCommand;
        
        public override async Task ExecuteAsync(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var msg = update.Message;

                if (msg.Type == MessageType.Document)
                {
                    var fileId = msg.Document.FileId;
                    var fileName = msg.Document.FileName;

                    // Скачиваем файл
                    var file = await _botClient.GetFileAsync(fileId);

                    var filePath = Path.Combine(Environment.CurrentDirectory, "Downloads");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    var localFilePath = Path.Combine(filePath, fileName);

                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await _botClient.DownloadFileAsync(file.FilePath, fileStream);
                    }
                    // Чтение содержимого файла
                    string fileContent = File.ReadAllText(localFilePath);

                    // Обрезаем слишком длинные файлы
                    const int maxLength = 4096; // Telegram limit for one message
                    if (fileContent.Length > maxLength)
                        fileContent = fileContent.Substring(0, maxLength) + "\n\n[Обрезано по лимиту Telegram]";
                    
                    var contentArr = fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    var trackcodes = new string[contentArr.Length-1];
                    var receivedDate = ServiceTools.PasrseDate(contentArr[0]);
                    Array.Copy(contentArr,1, trackcodes,0, trackcodes.Length);
                    await _trackingService.SetTrackingRange(trackcodes, receivedDate);
                    await _botClient.SendTextMessageAsync(msg.Chat.Id, $"Файл '{fileName}' успешно загружен! \n колво: {trackcodes.Length} \n Содержимое: {fileContent}");
                }
                else if (msg.Type == MessageType.Text)
                {
                    await _botClient.SendTextMessageAsync(msg.Chat.Id, "Пришли, пожалуйста, файл.");
                }
            }

            //const string message = "FILE PROCESSING";

            //await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
        }
    }
}
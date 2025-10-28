using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class FindByTrackingCodeCommand: BaseCommand
{
    private readonly ITrackingService _trackingService;
    private readonly TelegramBotClient _botClient;

    public FindByTrackingCodeCommand(ITrackingService trackingService, TelegramBot telegramBot)
    {
        _trackingService = trackingService;
        _botClient = telegramBot.GetBot().Result;
    }
    public override string Name => CommandNames.FindByTrackingCodeCommand;
    public override async Task ExecuteAsync(Update update)
    {
        var message = "Введите трек-код!";
        string code = update.Message.Text.Trim() == null ? string.Empty : update.Message.Text.Trim();
        if (code == string.Empty)
        {
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
            return;
        }
        var tracking = await _trackingService.GetTrackingByCode(code);
        if (tracking == null)
        {
            message = $"По трек-коду \"{code}\" товар не найден!";
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
            return;
        }

        if (tracking.GoodsReceived)
        {
            message = $"Товап с трек-кодом \"{ code }\" выдан {tracking.GoodReceivedDate.ToShortDateString()} -го числа!";
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
            return;
        }

        message = $"Товар с трек-кодом {code} будет доставлен {tracking.ReceivedDate.ToShortDateString()} -го числа!";
        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message,null, ParseMode.Markdown);
    }
}
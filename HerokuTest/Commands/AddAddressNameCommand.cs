using HerokuTest.Entities;
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HerokuTest.Commands;

public class AddAddressNameCommand : BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;

    public AddAddressNameCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.AddAddressNameCommand;

    public override async Task ExecuteAsync(Update update, AppUser appUser)
    {
        var username = update.Message.Text.Trim() == null ? string.Empty : update.Message.Text.Trim();
        await _userService.SetUserGenAddressName(appUser, username);
        await _botClient.SendTextMessageAsync(appUser.ChatId, $"Введите номер телефона! \nПример: 119001310");
    }
}
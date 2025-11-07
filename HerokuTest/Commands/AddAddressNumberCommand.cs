using HerokuTest.Entities;
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;

namespace HerokuTest.Commands;

public class AddAddressNumberCommand : BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;

    public AddAddressNumberCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.AddAddressNumberCommand;

    public override async Task ExecuteAsync(Update update, AppUser appUser)
    {
        var text = update.Message.Text.Trim() == null ? string.Empty : update.Message.Text.Trim();
        var number = Regex.Replace(text, @"\D", "");
        if (number.Length < 9)
        {
            await _botClient.SendTextMessageAsync(appUser.ChatId, $"Номер введен не правильно {number}");
        }
        var last9 = number[^9..];
        await _userService.SetUserGenAddressNumber(appUser, last9);
        var result = "ArzonCargo-tj\n19972639805\n" +
                     "浙江省义乌市福田街道 湖塘通福5区 21栋3单元123仓库(A192KZ)\n" +
                     $"[{appUser.GenAddressName}|{appUser.GenAddressNumber}]";
        await _botClient.SendTextMessageAsync(appUser.ChatId, result);
    }
}
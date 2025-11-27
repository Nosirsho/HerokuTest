using HerokuTest.Entities;
using HerokuTest.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Commands;

public class GetGenerateAddressCommand : BaseCommand
{
    private readonly IUserService _userService;
    private readonly TelegramBotClient _botClient;

    public GetGenerateAddressCommand(TelegramBot telegramBot, IUserService userService)
    {
        _userService = userService;
        _botClient = telegramBot.GetBot().Result;
    }

    public override string Name => CommandNames.GetGenerateAddressCommand;

    public override async Task ExecuteAsync(Update update, AppUser appUser)
    {
        var result = "";
        var addressName = appUser.GenAddressName;
        var addressNumber = appUser.GenAddressNumber;

        if (addressName.Length == 0 && addressNumber.Length == 0)
        {
            result = $"Введите имя!";
            await _userService.SetUserLastCommand(appUser, CommandNames.GetGenerateAddressCommand);
        } else if (addressName.Length > 0 && addressNumber.Length == 0)
        {
            result = $"Введите номер телефона! \nПример: 119001310";
            await _userService.SetUserLastCommand(appUser, CommandNames.AddAddressNameCommand);
        } else if (addressName.Length > 0 && addressNumber.Length > 0)
        {
            result = "ArzonCargo-tj\n19972639805\n" +
                     "浙江省义乌市福田街道 湖塘通福5区 21栋3单元123仓库(A192KZ)\n" +
                     $"({addressName} | {addressNumber})";
        }
        await _botClient.SendTextMessageAsync(appUser.ChatId, result,null,
            parseMode: ParseMode.Html);
    }
}
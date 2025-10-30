using HerokuTest.Commands;
using HerokuTest.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IUserService _userService;
        private readonly List<BaseCommand> _commands;
        private BaseCommand _lastCommand;
        private TelegramBotClient _botClient;
        

        public CommandExecutor(IServiceProvider serviceProvider, IUserService userService)
        {
            _userService = userService;
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        
        public async Task Execute(Update update)
        {
            if(update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;
            
            var appUser = await _userService.GetOrCreate(update);
            
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message?.Text)
                {
                    case "Расценки и сроки доставки":
                        await ExecuteCommand(CommandNames.GetPriceDeliveryCommand, update, appUser);
                        return;
                    case "Запрещенные товары":
                        await ExecuteCommand(CommandNames.GetProhibitedGoodsCommand, update, appUser);
                        return;
                    case "Программы для установки":
                        await ExecuteCommand(CommandNames.GetMarketAppsCommand, update, appUser);
                        return;
                    case "Контакты":
                        await ExecuteCommand(CommandNames.GetContactsCommand, update, appUser);
                        return;
                    case "Поиск по трек-коду":
                        await ExecuteCommand(CommandNames.GetTrackingCodeCommand, update, appUser);
                        return;
                    case "Добавление трек-кода":
                        await ExecuteCommand(CommandNames.AddTrackingCodeCommand, update, appUser);
                        return;
                    case "Доб. полученные трек-коды":
                        await ExecuteCommand(CommandNames.AddReceivedTrackingCodeCommand, update, appUser);
                        return;
                    case "Доб. админ":
                        await ExecuteCommand(CommandNames.AddAdminCommand, update, appUser);
                        return;
                }
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                if (update.CallbackQuery.Data.Contains("analytic"))
                {
                    await ExecuteCommand(CommandNames.GetAnalyticsCommand, update, appUser);
                    return;
                }
            }
            
            if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(CommandNames.StartCommand))
            {
                await ExecuteCommand(CommandNames.StartCommand, update, appUser);
                return;
            }
            if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(CommandNames.AdminCommand))
            {
                await ExecuteCommand(CommandNames.AdminCommand, update, appUser);
                return;
            }
            if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(CommandNames.AddFirstAdminCommand))
            {
                await ExecuteCommand(CommandNames.AddFirstAdminCommand, update, appUser);
                return;
            }

            // AddOperation => SelectCategory => FinishOperation
            switch (appUser.LastCommand)
            {
                case CommandNames.AddOperationCommand:
                {
                    await ExecuteCommand(CommandNames.SelectCategoryCommand, update, appUser);
                    break;
                }
                case CommandNames.SelectCategoryCommand:
                {
                    await ExecuteCommand(CommandNames.FinishOperationCommand, update, appUser);
                    break;
                }
                case CommandNames.GetTrackingCodeCommand:
                {
                    await ExecuteCommand(CommandNames.FindByTrackingCodeCommand, update, appUser);
                    break;
                }
                case CommandNames.AddTrackingCodeCommand:
                {
                    await ExecuteCommand(CommandNames.AddFileProcessCommand, update, appUser);
                    break;
                }
                case CommandNames.AddReceivedTrackingCodeCommand:
                {
                    await ExecuteCommand(CommandNames.AddReceivedFileProcessCommand, update, appUser);
                    break;
                }
                case CommandNames.AddAdminCommand:
                {
                    await ExecuteCommand(CommandNames.AddAdminExecuteCommand, update, appUser);
                    break;
                }
                case null:
                {
                    await ExecuteCommand(CommandNames.StartCommand, update, appUser);
                    break;
                }
            }
        }

        private async Task ExecuteCommand(string commandName, Update update, AppUser appUser)
        {
            var user = await _userService.SetUserLastCommand(appUser, commandName);
            _lastCommand = _commands.First(x => x.Name == user.LastCommand);
            await _lastCommand.ExecuteAsync(update);
        }
    }
}
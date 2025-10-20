using HerokuTest.Commands;
using HerokuTest.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly List<BaseCommand> _commands;
        private BaseCommand _lastCommand;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _commands = serviceProvider.GetServices<BaseCommand>().ToList();
        }
        
        public async Task Execute(Update update)
        {
            if(update?.Message?.Chat == null && update?.CallbackQuery == null)
                return;

            // if (update.Type == UpdateType.Message)
            // {
            //     switch (update.Message?.Text)
            //     {
            //         case "Расценки и сроки доставки":
            //             await ExecuteCommand(CommandNames.GetPriceDeliveryCommand, update);
            //             return;
            //         case "Запрещенные товары":
            //             await ExecuteCommand(CommandNames.GetProhibitedGoodsCommand, update);
            //             return;
            //         case "Программы для установки":
            //             await ExecuteCommand(CommandNames.GetMarketAppsCommand, update);
            //             return;
            //         case "Контакты":
            //             await ExecuteCommand(CommandNames.GetContactsCommand, update);
            //             return;
            //         case "Поиск по трек-коду":
            //             await ExecuteCommand(CommandNames.GetTrackingCodeCommand, update);
            //             return;
            //         case "Добавление трек-кода":
            //             await ExecuteCommand(CommandNames.AddTrackingCodeCommand, update);
            //             return;
            //         case "Доб. полученные трек-коды":
            //             await ExecuteCommand(CommandNames.AddReceivedTrackingCodeCommand, update);
            //             return;
            //     }
            // }

            // if (update.Type == UpdateType.CallbackQuery)
            // {
            //     if (update.CallbackQuery.Data.Contains("analytic"))
            //     {
            //         await ExecuteCommand(CommandNames.GetAnalyticsCommand, update);
            //         return;
            //     }
            // }
            
            if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(CommandNames.StartCommand))
            {
                await ExecuteCommand(CommandNames.StartCommand, update);
                return;
            }
            // if (update.Message != null && update.Message.Text != null && update.Message.Text.Contains(CommandNames.AdminCommand))
            // {
            //     await ExecuteCommand(CommandNames.AdminCommand, update);
            //     return;
            // }
            //
            // // AddOperation => SelectCategory => FinishOperation
            // switch (_lastCommand?.Name)
            // {
            //     case CommandNames.AddOperationCommand:
            //     {
            //         await ExecuteCommand(CommandNames.SelectCategoryCommand, update);
            //         break;
            //     }
            //     case CommandNames.SelectCategoryCommand:
            //     {
            //         await ExecuteCommand(CommandNames.FinishOperationCommand, update);
            //         break;
            //     }
            //     case CommandNames.GetTrackingCodeCommand:
            //     {
            //         await ExecuteCommand(CommandNames.FindByTrackingCodeCommand, update);
            //         break;
            //     }
            //     case CommandNames.AddTrackingCodeCommand:
            //     {
            //         await ExecuteCommand(CommandNames.AddFileProcessCommand, update);
            //         break;
            //     }
            //     case CommandNames.AddReceivedTrackingCodeCommand:
            //     {
            //         await ExecuteCommand(CommandNames.AddReceivedFileProcessCommand, update);
            //         break;
            //     }
            //     case CommandNames.FindByTrackingCodeCommand:
            //     {
            //         await ExecuteCommand(CommandNames.FindByTrackingCodeCommand, update);
            //         break;
            //     }
            //     case null:
            //     {
            //         await ExecuteCommand(CommandNames.StartCommand, update);
            //         break;
            //     }
            // }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            _lastCommand = _commands.First(x => x.Name == commandName);
            await _lastCommand.ExecuteAsync(update);
        }
    }
}
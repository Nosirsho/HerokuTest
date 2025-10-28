using HerokuTest.Entities;
using Telegram.Bot.Types;

namespace HerokuTest.Services;

public interface IUserService
{
    Task<AppUser> GetOrCreate(Update update);
    Task<AppUser> SetUserLastCommand(AppUser appUser, string command);
}
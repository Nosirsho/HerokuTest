using HerokuTest.Entities;
using Telegram.Bot.Types;

namespace HerokuTest.Services;

public interface IUserService
{
    Task<AppUser> GetOrCreate(Update update);
    Task<AppUser?> GetUserByUsername(string username);
    Task<AppUser> SetUserLastCommand(AppUser appUser, string command);
    Task<AppUser> AddAdmin(AppUser appUser);
    Task<AppUser?> HasAdmin();
}
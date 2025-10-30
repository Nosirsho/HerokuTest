using HerokuTest.Entities;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HerokuTest.Services;

public class UserService: IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }
    
    public async Task<AppUser> GetOrCreate(Update update)
    {
        var newUser = update.Type switch
        {
            UpdateType.CallbackQuery => new AppUser
            {
                Username = update.CallbackQuery.From.Username,
                ChatId = update.CallbackQuery.Message.Chat.Id,
                FirstName = update.CallbackQuery.Message.From.FirstName,
                LastName = update.CallbackQuery.Message.From.LastName
            },
            UpdateType.Message => new AppUser
            {
                Username = update.Message.Chat.Username,
                ChatId = update.Message.Chat.Id,
                FirstName = update.Message.Chat.FirstName,
                LastName = update.Message.Chat.LastName ?? ""
            }
        };
        var user = await _context.Users.Where(x => x.ChatId == newUser.ChatId).FirstOrDefaultAsync();
            
        if (user != null) return user;
            
        var result = await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<AppUser?> GetUserByUsername(string username)
    {
        var appUser = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        return appUser;
    }

    public async Task<AppUser> SetUserLastCommand(AppUser appUser, string command)
    {
        var user = await _context.Users.Where(x => x.ChatId == appUser.ChatId).FirstOrDefaultAsync();
        user!.LastCommand = command;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<AppUser> AddAdmin(AppUser appUser)
    {
        var user = await _context.Users.Where(x => x.ChatId == appUser.ChatId).FirstOrDefaultAsync();
        user!.IsAdmin = true;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<AppUser?> HasAdmin()
    {
        var user = await _context.Users.Where(u=>u.IsAdmin).FirstOrDefaultAsync();
        return user;
    }
}
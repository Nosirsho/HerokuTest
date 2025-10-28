using Microsoft.AspNetCore.Mvc;

namespace HerokuTest.Entities;

public class AppUser: BaseEntity
{
    public long ChatId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;
    public string LastCommand { get; set; } = string.Empty;
}
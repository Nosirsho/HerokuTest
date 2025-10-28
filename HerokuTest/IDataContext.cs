using HerokuTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace HerokuTest;

public interface IDataContext
{
    public DbSet<AppUser> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
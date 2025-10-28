using HerokuTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace HerokuTest;

public class DataContext: DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
        
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Tracking> Trackings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Однократное заполнение таблицы YourEntities
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { Id = 1, Username = "Nx1ze", IsAdmin = true },
            new AppUser { Id = 2, Username = "chudotovartajikistan", IsAdmin = true }
        );
    }

}
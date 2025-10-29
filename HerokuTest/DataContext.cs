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

        var createdAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
    }

}
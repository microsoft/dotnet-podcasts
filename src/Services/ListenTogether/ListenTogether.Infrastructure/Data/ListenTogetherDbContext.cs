using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.EntityFrameworkCore;

namespace ListenTogether.Infrastructure.Data;

public class ListenTogetherDbContext : DbContext, IApplicationDbContext
{
    public ListenTogetherDbContext(DbContextOptions options) : base(options)
    {
            
    }

    public DbSet<Room> Rooms => Set<Room>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasKey(prop => prop.Code);
        modelBuilder.Entity<Room>().HasMany(room => room.Users).WithOne();
        modelBuilder.Entity<Episode>().HasKey(e => e.Id);
        modelBuilder.Entity<Episode>().ToTable("Episodes").HasOne(e=> e.Show).WithMany();
        modelBuilder.Entity<Show>().ToTable("Shows").HasKey(prop => prop.Id);
        modelBuilder.Entity<User>().ToTable("Users").HasKey(prop => prop.Id);
        base.OnModelCreating(modelBuilder);
    }
}
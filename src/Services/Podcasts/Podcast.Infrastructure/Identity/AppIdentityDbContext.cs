using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Podcast.Infrastructure.Identity.Models;

namespace Podcast.Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext
{
    public AppIdentityDbContext()
    {
    }
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Seed.SeedUsers(builder);
        Seed.SeedRoles(builder);
        Seed.SeedUserRoles(builder);
    }
}


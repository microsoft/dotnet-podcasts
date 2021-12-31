using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Podcast.Infrastructure.Identity.Models;

namespace Podcast.Infrastructure.Identity;

internal static class Seed
{
    internal static void SeedUsers(ModelBuilder builder)
    {
        ApplicationUser admin = new();
        PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

        admin.Id = "b74ddd14-6340-4840-95c2-db12554843e5";
        admin.UserName = "Admin";
        admin.Email = "admin@admin.com";
        admin.LockoutEnabled = false;
        admin.PhoneNumber = "1234567890";
        admin.NormalizedEmail = "admin@admin.com".ToUpper();
        admin.NormalizedUserName = "Admin".ToUpper();
        admin.EmailConfirmed = true;
        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin*123");


        ApplicationUser normalUser = new();
        normalUser.Id = "82421986-4550-4c1b-9f07-1b9f43d81e0b";
        normalUser.UserName = "AppUser";
        normalUser.Email = "user@user.com";
        normalUser.LockoutEnabled = false;
        normalUser.PhoneNumber = "1234567890";
        normalUser.NormalizedEmail = "user@user.com".ToUpper();
        normalUser.NormalizedUserName = "AppUser".ToUpper();
        normalUser.EmailConfirmed = true;
        normalUser.PasswordHash = passwordHasher.HashPassword(normalUser, "User*123");

        builder.Entity<ApplicationUser>().HasData(admin);
        builder.Entity<ApplicationUser>().HasData(normalUser);
    }

    internal static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new()
            {
                Id = "fab4fac1-c546-41de-aebc-a14da6895711",
                Name = "Admin",
                ConcurrencyStamp = "1",
                NormalizedName = "Admin"
            },
            new()
            {
                Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                Name = "User",
                ConcurrencyStamp = "2",
                NormalizedName = "User"
            });
    }
    internal static void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "fab4fac1-c546-41de-aebc-a14da6895711",
                UserId = "b74ddd14-6340-4840-95c2-db12554843e5"
            });
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "c7b013f0-5201-4317-abd8-c211f91b7330",
                UserId = "82421986-4550-4c1b-9f07-1b9f43d81e0b"
            });
    }

}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Podcast.Infrastructure.Identity.Models;
using System.Text;

namespace Podcast.Infrastructure.Identity;

public static class IdentityDependencyInjection
{
    public static void AddIdentityDI(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("IdentityDb")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 5;
            options.SignIn.RequireConfirmedEmail = false;

        }).AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = config["IdentitySettings:Audience"],
                ValidIssuer = config["IdentitySettings:Issuer"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["IdentitySettings:Key"])),
                ValidateIssuerSigningKey = true
            };
        });

    }
}

using API.Services;
using Aplicacion.UnitOfWork;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using iText.StyledXmlParser.Jsoup.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace API.Extensions;

public static class AppServiceExtension
{
    public static void ConfigureScoped(this IServiceCollection services){
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorizationHandler, GlobalVerbRolseHandler>();
    }
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration){
        services.Configure<JWT>(configuration.GetSection("JWT"));
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(e => {
            e.RequireHttpsMetadata = false;
            e.SaveToken = false;
            e.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer =true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            };
        });
    }
}

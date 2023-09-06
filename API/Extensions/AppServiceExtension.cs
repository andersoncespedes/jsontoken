using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.UnitOfWork;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static class AppServiceExtension
{
    public static void ConfigureScoped(this IServiceCollection services){
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        //services.AddScoped<IUserService, UserService>();
        //services.AddScoped<IAuthorizationHandler, GlobalVerbRoleHandler>();

    }
}

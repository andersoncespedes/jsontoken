using System.Net;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureScoped();

builder.Services.AddAuthorization(opts => {
    opts.DefaultPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .AddRequirements(new GlobalVerbRolseRequirement())
    .Build();
});
builder.Services.AddDbContext<APIContext>(option => {
    string conexion = builder.Configuration.GetConnectionString("DefaultConecction");
    option.UseMySql(conexion, ServerVersion.AutoDetect(conexion));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();
//app.UseAuthentication();
app.UseAuthorization();


app.Run();

using System.Reflection;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data;

public class APIContext : DbContext
{
    public DbSet<User> Users{ get; set; }
    public DbSet<UserRol> UserRols{get; set; }
    public DbSet<Rol> Rols { get; set; }
    public APIContext(DbContextOptions<APIContext> options) : base(options){

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

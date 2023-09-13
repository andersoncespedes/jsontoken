using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
       builder.ToTable("user");

       builder.Property(e => e.Username)
       .HasColumnName("username")
       .HasColumnType("varchar")
       .IsRequired()
       .HasMaxLength(50);

        builder.Property(e => e.Email)
        .HasColumnName("email")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(e => e.Password)
        .HasColumnName("password")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(250);

        builder.HasMany(e => e.Roles)
        .WithMany(e => e.Users)
        .UsingEntity<UserRols>(
            j => j.HasOne(e => e.Rol)
            .WithMany(e => e.UsersRoles)
            .HasForeignKey(e => e.RolId),

            j => j.HasOne(e => e.User)
            .WithMany(e => e.UsersRoles)
            .HasForeignKey(e => e.UserId),

            j => {
                j.ToTable("user_rol");
                j.HasKey(e => new {e.RolId, e.UserId});
            }
        );
    }
}

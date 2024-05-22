using System;
using Api.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = CustomRoles.Admin },
                new Role { Id = 2, Name = CustomRoles.User }
            );

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(entity => new { entity.UserId });
            entity.Property(e => e.EmailAddress).HasMaxLength(450).IsRequired();
            entity.HasIndex(e => e.EmailAddress).IsUnique();
            entity.Property(e => e.Password).IsRequired();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RoleId);
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

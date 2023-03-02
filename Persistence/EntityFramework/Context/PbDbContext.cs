using Domain.Agregates.UserAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Entities;
using System;

namespace Persistence.EntityFramework.Context
{
    public class PbDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public virtual DbSet<UserEntity> Users { get; set; }

        public virtual DbSet<UserRoleEntity> Roles { get; set; }

        public virtual DbSet<ProductCategoryEntity> Categories { get; set; }

        public virtual DbSet<ProductEntity> Products { get; set; }

        public PbDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRole = new UserRoleEntity()
            {
                Id = Guid.NewGuid().ToString(),
                RoleType = UserRoleType.Admin
            };

            var userRole = new UserRoleEntity()
            {
                Id = Guid.NewGuid().ToString(),
                RoleType = UserRoleType.User
            };

            var admin = new UserEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@email.com",
                LastName = "Admin",
                Name = "Admin",
                NickName = "Administrator",
                Password = "UXdlcnR5MTIzIQ==",
                Role = adminRole,
                SessionToken = Guid.NewGuid().ToString()
            };

            modelBuilder.Entity<UserEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserRoleEntity>()
                .Property(r => r.RoleType)
                .HasConversion<int>();

            modelBuilder.Entity<UserEntity>()
                .HasOne(s => s.Role)
                .WithMany(c => c.Users);

            modelBuilder.Entity<ProductEntity>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<ProductCategoryEntity>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);

            modelBuilder.Entity<UserRoleEntity>()
                .HasData(
                    adminRole,
                    userRole
                );

            modelBuilder.Entity<UserEntity>()
                .HasData(
                    new
                    {
                        Id = admin.Id,
                        Email = admin.Email,
                        LastName = admin.LastName,
                        Name = admin.Name,
                        NickName = admin.NickName,
                        Password = admin.Password,
                        RoleId = adminRole.Id,
                        SessionToken = adminRole.Id
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                $@"Data Source={_configuration["Db:DataSource"]}",
                b => b.MigrationsAssembly("PB_WebApi"));
        }


    }
}

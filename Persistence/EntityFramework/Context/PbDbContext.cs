using Contracts;
using Domain.Aggregates.UserAggregate;
using Domain.Utils;
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

            modelBuilder.Entity<UserEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserEntity>()
                .Property(r => r.RepositoryType)
                .HasConversion<int>();

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
                        Id = Guid.NewGuid().ToString(),
                        Email = _configuration["Admin:email"],
                        LastName = "Administrator",
                        Name = "Administrator",
                        NickName = "Administrator",
                        Password = EncodingUtils.EncodeData(_configuration["Admin:password"]),
                        RoleId = adminRole.Id,
                        SessionToken = Guid.NewGuid().ToString(),
                        RepositoryType = RepositoryType.EntityFramework
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration["Db:ConnectionString"],
                b => b.MigrationsAssembly(_configuration["Db:MigrationAssembly"]));
        }
    }
}

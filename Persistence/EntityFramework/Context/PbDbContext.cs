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

        public PbDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasOne(s => s.Role)
                .WithMany(c => c.Users);

            modelBuilder.Entity<UserRoleEntity>()
                .HasData(

                new UserRoleEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleType = UserRoleType.User
                },

                new UserRoleEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleType = UserRoleType.Admin
                }

                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={_configuration["Db:DataSource"]}", b => b.MigrationsAssembly("PB_WebApi"));
        }

        public virtual DbSet<UserEntity> Users { get; set; }

        public virtual DbSet<UserRoleEntity> Roles { get; set; }
    }
}

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.EntityFramework.Context;

#nullable disable

namespace PB_WebApi.Migrations
{
    [DbContext(typeof(PbDbContext))]
    [Migration("20230302022051_add-conversion")]
    partial class addconversion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.14");

            modelBuilder.Entity("Domain.Agregates.ProductAgregate.ProductCategoryEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Agregates.ProductAgregate.ProductEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Persistence.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionToken")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "09886728-fc04-4aac-8ace-fef1b3615bb9",
                            Email = "admin@email.com",
                            LastName = "Admin",
                            Name = "Admin",
                            NickName = "Administrator",
                            Password = "UXdlcnR5MTIzIQ==",
                            RoleId = "e99a720a-dbf3-4b92-b264-61db0d8feddb",
                            SessionToken = "e99a720a-dbf3-4b92-b264-61db0d8feddb"
                        });
                });

            modelBuilder.Entity("Persistence.Entities.UserRoleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = "e99a720a-dbf3-4b92-b264-61db0d8feddb",
                            RoleType = 1
                        },
                        new
                        {
                            Id = "de5ae337-d4e9-47e2-9195-9823fff52693",
                            RoleType = 0
                        });
                });

            modelBuilder.Entity("ProductCategoryEntityProductEntity", b =>
                {
                    b.Property<string>("CategoriesId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("ProductCategoryEntityProductEntity");
                });

            modelBuilder.Entity("Persistence.Entities.UserEntity", b =>
                {
                    b.HasOne("Persistence.Entities.UserRoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ProductCategoryEntityProductEntity", b =>
                {
                    b.HasOne("Domain.Agregates.ProductAgregate.ProductCategoryEntity", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Agregates.ProductAgregate.ProductEntity", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Persistence.Entities.UserRoleEntity", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using GreenThumb.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GreenThumb.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231205152243_AddEncryption")]
    partial class AddEncryption
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GreenThumb.Models.GardenModel", b =>
                {
                    b.Property<int>("GardenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GardenId"), 1L, 1);

                    b.Property<string>("GardenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("garden_name");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("GardenId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Gardens");
                });

            modelBuilder.Entity("GreenThumb.Models.GardenPlant", b =>
                {
                    b.Property<int>("GardenId")
                        .HasColumnType("int")
                        .HasColumnName("garden_id");

                    b.Property<int>("PlantId")
                        .HasColumnType("int")
                        .HasColumnName("plant_id");

                    b.HasKey("GardenId", "PlantId");

                    b.HasIndex("PlantId");

                    b.ToTable("GardenPlants");
                });

            modelBuilder.Entity("GreenThumb.Models.InstructionModel", b =>
                {
                    b.Property<int>("InstructionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstructionId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("PlantId")
                        .HasColumnType("int")
                        .HasColumnName("plant_id");

                    b.HasKey("InstructionId");

                    b.HasIndex("PlantId");

                    b.ToTable("Instructions");

                    b.HasData(
                        new
                        {
                            InstructionId = 1,
                            Description = "Should be planted in sour soil",
                            PlantId = 1
                        },
                        new
                        {
                            InstructionId = 2,
                            Description = "Requires a lot of water",
                            PlantId = 1
                        },
                        new
                        {
                            InstructionId = 3,
                            Description = "Prune down to bottom each winter",
                            PlantId = 2
                        },
                        new
                        {
                            InstructionId = 4,
                            Description = "Prune down fruit giving flower shoots after harvest",
                            PlantId = 3
                        },
                        new
                        {
                            InstructionId = 5,
                            Description = "Plant in sunny location",
                            PlantId = 3
                        },
                        new
                        {
                            InstructionId = 6,
                            Description = "Needs to be protected from birds",
                            PlantId = 3
                        },
                        new
                        {
                            InstructionId = 7,
                            Description = "Should be replaced every 4-5 year",
                            PlantId = 4
                        },
                        new
                        {
                            InstructionId = 8,
                            Description = "Requires a lot of pest control",
                            PlantId = 5
                        },
                        new
                        {
                            InstructionId = 9,
                            Description = "Plant in sunny location",
                            PlantId = 5
                        },
                        new
                        {
                            InstructionId = 10,
                            Description = "Don't fertilize during summer months",
                            PlantId = 5
                        });
                });

            modelBuilder.Entity("GreenThumb.Models.PlantModel", b =>
                {
                    b.Property<int>("PlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<DateTime>("PlantDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("plant_date");

                    b.HasKey("PlantId");

                    b.ToTable("Plants");

                    b.HasData(
                        new
                        {
                            PlantId = 1,
                            Name = "Rhododendron",
                            PlantDate = new DateTime(2023, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PlantId = 2,
                            Name = "Autumn raspberries",
                            PlantDate = new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PlantId = 3,
                            Name = "Summer raspberries",
                            PlantDate = new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PlantId = 4,
                            Name = "Strawberries",
                            PlantDate = new DateTime(2022, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            PlantId = 5,
                            Name = "Elderflower",
                            PlantDate = new DateTime(2010, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("GreenThumb.Models.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GreenThumb.Models.GardenModel", b =>
                {
                    b.HasOne("GreenThumb.Models.UserModel", "User")
                        .WithOne("Garden")
                        .HasForeignKey("GreenThumb.Models.GardenModel", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GreenThumb.Models.GardenPlant", b =>
                {
                    b.HasOne("GreenThumb.Models.GardenModel", "Garden")
                        .WithMany("GardenPlants")
                        .HasForeignKey("GardenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GreenThumb.Models.PlantModel", "Plant")
                        .WithMany("GardenPlants")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Garden");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("GreenThumb.Models.InstructionModel", b =>
                {
                    b.HasOne("GreenThumb.Models.PlantModel", "Plant")
                        .WithMany("Instructions")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("GreenThumb.Models.GardenModel", b =>
                {
                    b.Navigation("GardenPlants");
                });

            modelBuilder.Entity("GreenThumb.Models.PlantModel", b =>
                {
                    b.Navigation("GardenPlants");

                    b.Navigation("Instructions");
                });

            modelBuilder.Entity("GreenThumb.Models.UserModel", b =>
                {
                    b.Navigation("Garden");
                });
#pragma warning restore 612, 618
        }
    }
}

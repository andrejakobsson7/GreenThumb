using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using GreenThumb.Managers;
using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenThumb.Database
{
    public class AppDbContext : DbContext
    {
        private readonly IEncryptionProvider _greenThumbEncryption;

        public AppDbContext()
        {
            _greenThumbEncryption = new GenerateEncryptionProvider(KeyManager.GetEncryptionKey());
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<GardenModel> Gardens { get; set; }
        public DbSet<PlantModel> Plants { get; set; }
        public DbSet<GardenPlant> GardenPlants { get; set; }
        public DbSet<InstructionModel> Instructions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GreenThumbDb;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Define combined key for joint table GardenPlant
            modelBuilder.Entity<GardenPlant>(entity =>
            {
                entity.HasKey(entity => new { entity.GardenId, entity.PlantId });
            });

            //Define encryption
            modelBuilder.UseEncryption(_greenThumbEncryption);

            //Seed data
            modelBuilder.Entity<PlantModel>()
                .HasData(
                new PlantModel()
                {
                    PlantId = 1,
                    Name = "Rhododendron",
                    PlantDate = new DateTime(2023, 06, 30),
                    ImageUrl = "C:\\Users\\andre\\OneDrive\\Dokument\\Databas\\Övningar\\GreenThumb\\GreenThumb\\bin\\Debug\\net8.0-windows\\01c17930-01db-43b9-8228-eeb5e8a67151.jpg"
                },
                new PlantModel()
                {
                    PlantId = 2,
                    Name = "Autumn raspberries",
                    PlantDate = new DateTime(2023, 04, 01),
                    ImageUrl = "C:\\Users\\andre\\OneDrive\\Dokument\\Databas\\Övningar\\GreenThumb\\GreenThumb\\bin\\Debug\\net8.0-windows\\67b6728d-1887-4782-8fe3-34ed82f89ad3.jpg"
                },
                new PlantModel()
                {
                    PlantId = 3,
                    Name = "Summer raspberries",
                    PlantDate = new DateTime(2022, 05, 05),
                    ImageUrl = "C:\\Users\\andre\\OneDrive\\Dokument\\Databas\\Övningar\\GreenThumb\\GreenThumb\\bin\\Debug\\net8.0-windows\\8c02969f-a649-4f4e-9fad-3e4e111bf00a.jpg"
                },
                new PlantModel()
                {
                    PlantId = 4,
                    Name = "Strawberries",
                    PlantDate = new DateTime(2022, 05, 06),
                    ImageUrl = "C:\\Users\\andre\\OneDrive\\Dokument\\Databas\\Övningar\\GreenThumb\\GreenThumb\\bin\\Debug\\net8.0-windows\\acac1f86-b7f6-4690-9563-82072c15118e.jpg"
                },
                new PlantModel()
                {
                    PlantId = 5,
                    Name = "Elderflower",
                    PlantDate = new DateTime(2010, 10, 10),
                    ImageUrl = "C:\\Users\\andre\\OneDrive\\Dokument\\Databas\\Övningar\\GreenThumb\\GreenThumb\\bin\\Debug\\net8.0-windows\\0c87edda-ddd0-4d84-abfe-e322549c8d91.jpg"
                }
                );

            modelBuilder.Entity<InstructionModel>()
                .HasData(
                new InstructionModel()
                {
                    InstructionId = 1,
                    Description = "Should be planted in sour soil",
                    PlantId = 1
                },
                new InstructionModel()
                {
                    InstructionId = 2,
                    Description = "Requires a lot of water",
                    PlantId = 1
                },
                new InstructionModel()
                {
                    InstructionId = 3,
                    Description = "Prune down to bottom each winter",
                    PlantId = 2
                },
                new InstructionModel()
                {
                    InstructionId = 4,
                    Description = "Prune down fruit giving flower shoots after harvest",
                    PlantId = 3
                },
                new InstructionModel()
                {
                    InstructionId = 5,
                    Description = "Plant in sunny location",
                    PlantId = 3
                },
                new InstructionModel()
                {
                    InstructionId = 6,
                    Description = "Needs to be protected from birds",
                    PlantId = 3
                },
                new InstructionModel()
                {
                    InstructionId = 7,
                    Description = "Should be replaced every 4-5 year",
                    PlantId = 4
                },
                new InstructionModel()
                {
                    InstructionId = 8,
                    Description = "Requires a lot of pest control",
                    PlantId = 5
                },
                new InstructionModel()
                {
                    InstructionId = 9,
                    Description = "Plant in sunny location",
                    PlantId = 5
                },
                new InstructionModel()
                {
                    InstructionId = 10,
                    Description = "Don't fertilize during summer months",
                    PlantId = 5
                }
                );
        }
    }
}

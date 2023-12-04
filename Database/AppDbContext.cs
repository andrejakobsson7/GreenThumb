using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenThumb.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

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

            //Seed data
            modelBuilder.Entity<PlantModel>()
                .HasData(
                new PlantModel()
                {
                    PlantId = 1,
                    Name = "Rhododendron",
                    PlantDate = new DateTime(2023, 06, 30),
                },
                new PlantModel()
                {
                    PlantId = 2,
                    Name = "Autumn raspberries",
                    PlantDate = new DateTime(2023, 04, 01),
                },
                new PlantModel()
                {
                    PlantId = 3,
                    Name = "Summer raspberries",
                    PlantDate = new DateTime(2022, 05, 05),
                },
                new PlantModel()
                {
                    PlantId = 4,
                    Name = "Strawberries",
                    PlantDate = new DateTime(2022, 05, 06),
                },
                new PlantModel()
                {
                    PlantId = 5,
                    Name = "Elderflower",
                    PlantDate = new DateTime(2010, 10, 10)
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

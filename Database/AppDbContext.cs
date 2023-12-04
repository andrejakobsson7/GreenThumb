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
        }
    }
}

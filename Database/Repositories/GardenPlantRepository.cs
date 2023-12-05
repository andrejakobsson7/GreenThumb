using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace GreenThumb.Database.Repositories
{
    public class GardenPlantRepository
    {
        private AppDbContext _context;

        public GardenPlantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GardenPlant>> GetAllGardensByGardenIdWithRelatedDataAsync(int gardenId)
        {
            return await _context.GardenPlants
                .Include(gp => gp.Plant)
                .Include(gp => gp.Plant.Instructions)
                .Where(g => g.GardenId == gardenId)
                .ToListAsync();
        }
        async public Task AddGardenPlantAsync(GardenPlant newGardenPlant)
        {
            await _context.GardenPlants.AddAsync(newGardenPlant);
        }
        async public Task<GardenPlant?> GetGardenPlantById(int gardenId, int plantId)
        {
            return await _context.GardenPlants.FirstOrDefaultAsync(gp => gp.GardenId == gardenId && gp.PlantId == plantId);
        }
        async public Task RemoveGardenPlant(int gardenId, int plantId)
        {
            var gardenPlantToRemove = await GetGardenPlantById(gardenId, plantId);
            if (gardenPlantToRemove != null)
            {
                _context.GardenPlants.Remove(gardenPlantToRemove);
            }
            else
            {
                MessageBox.Show("Plant could not be located in garden!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

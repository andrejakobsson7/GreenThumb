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

        //Query to retrieve all the user's personal plants.
        //Include plant and it's instructions to be able to properly navigate to PlantWindow from here and see all necessary info.
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
        async public Task<bool> RemoveGardenPlantAsync(int gardenId, int plantId)
        {
            var gardenPlantToRemove = await GetGardenPlantById(gardenId, plantId);
            if (gardenPlantToRemove != null)
            {
                _context.GardenPlants.Remove(gardenPlantToRemove);
                return true;
            }
            else
            {
                MessageBox.Show("Plant could not be located in garden!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}

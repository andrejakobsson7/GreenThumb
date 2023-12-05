using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace GreenThumb.Database.Repositories
{
    public class PlantModelRepository
    {
        private AppDbContext _context;

        public PlantModelRepository(AppDbContext context)
        {
            _context = context;
        }

        async public Task<List<PlantModel>> GetAllPlantsAsync()
        {
            return await _context.Plants.ToListAsync();
        }

        async public Task<List<PlantModel>> GetPlantsByNameAsync(string plantName)
        {
            return await _context.Plants.Where(p => p.Name.Contains(plantName)).ToListAsync();
        }

        async public Task<PlantModel?> GetPlantByIdAsync(int plantId)
        {
            return await _context.Plants.FirstOrDefaultAsync(p => p.PlantId == plantId);
        }
        async public Task RemovePlant(int plantId)
        {
            var plantToRemove = await GetPlantByIdAsync(plantId);
            if (plantToRemove != null)
            {
                _context.Plants.Remove(plantToRemove);
            }
            else
            {
                MessageBox.Show("Plant could not be removed, try again later!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        async public Task AddPlantAsync(PlantModel newPlant)
        {
            await _context.Plants.AddAsync(newPlant);
        }
    }
}

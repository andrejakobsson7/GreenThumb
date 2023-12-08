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

        //Method used for getting all plants in the database. Include instructions so they can be displayed in 'ManagePlantWindow'.
        async public Task<List<PlantModel>> GetAllPlantsWithIncludedDataAsync()
        {
            return await _context.Plants.
                Include(p => p.Instructions).
                ToListAsync();
        }

        //Method used for searching on plant name. Include instructions so they can be displayed in 'ManagePlantWindow'.
        async public Task<List<PlantModel>> GetPlantsByNameAsync(string plantName)
        {
            return await _context.Plants.
                Include(p => p.Instructions).
                Where(p => p.Name.Contains(plantName)).
                ToListAsync();
        }

        //Method used for making sure that another plant with same name is not added again.
        async public Task<PlantModel?> GetPlantByName(string plantName)
        {
            return await _context.Plants.
                FirstOrDefaultAsync(p => p.Name == plantName);
        }

        async public Task<PlantModel?> GetPlantByIdAsync(int plantId)
        {
            return await _context.Plants.
                FirstOrDefaultAsync(p => p.PlantId == plantId);
        }
        async public Task<bool> RemovePlantAsync(int plantId)
        {
            var plantToRemove = await GetPlantByIdAsync(plantId);
            if (plantToRemove != null)
            {
                _context.Plants.Remove(plantToRemove);
                return true;
            }
            else
            {
                MessageBox.Show("Plant could not be removed, try again later!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        async public Task AddPlantAsync(PlantModel newPlant)
        {
            await _context.Plants.
                AddAsync(newPlant);
        }
    }
}

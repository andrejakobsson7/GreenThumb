using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace GreenThumb.Database.Repositories
{
    public class GardenModelRepository
    {
        private AppDbContext _context;

        public GardenModelRepository(AppDbContext context)
        {
            _context = context;
        }

        async public Task<GardenModel?> GetGardenByIdAsync(int gardenId)
        {
            return await _context.Gardens.FirstOrDefaultAsync(g => g.GardenId == gardenId);
        }
        async public Task UpdateGardenNameAsync(GardenModel garden, string newGardenName)
        {
            var gardenToUpdate = await GetGardenByIdAsync(garden.GardenId);
            if (gardenToUpdate != null)
            {
                gardenToUpdate.GardenName = newGardenName;
            }
            else
            {
                MessageBox.Show("Garden could not be located", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

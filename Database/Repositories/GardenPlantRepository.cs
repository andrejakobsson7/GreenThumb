namespace GreenThumb.Database.Repositories
{
    public class GardenPlantRepository
    {
        private AppDbContext _context;

        public GardenPlantRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

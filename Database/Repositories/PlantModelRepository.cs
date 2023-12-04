namespace GreenThumb.Database.Repositories
{
    public class PlantModelRepository
    {
        private AppDbContext _context;

        public PlantModelRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

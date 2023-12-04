namespace GreenThumb.Database.Repositories
{
    public class GardenModelRepository
    {
        private AppDbContext _context;

        public GardenModelRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

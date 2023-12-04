namespace GreenThumb.Database.Repositories
{
    public class UserModelRepository
    {
        private AppDbContext _context;

        public UserModelRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

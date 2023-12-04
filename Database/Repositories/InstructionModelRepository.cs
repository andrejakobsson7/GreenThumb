namespace GreenThumb.Database.Repositories
{
    public class InstructionModelRepository
    {
        private AppDbContext _context;

        public InstructionModelRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

using GreenThumb.Models;

namespace GreenThumb.Database.Repositories
{
    public class InstructionModelRepository
    {
        private AppDbContext _context;

        public InstructionModelRepository(AppDbContext context)
        {
            _context = context;
        }

        async public Task AddInstructionAsync(InstructionModel newInstruction)
        {
            await _context.Instructions.AddAsync(newInstruction);
        }
    }
}

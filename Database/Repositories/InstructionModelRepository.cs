using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;

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
        async public Task RemoveInstructionsByPlantId(int plantId)
        {
            var instructionsToRemove = await _context.Instructions.Where(i => i.PlantId == plantId).ToListAsync();
            foreach (var instruction in instructionsToRemove)
            {
                _context.Instructions.Remove(instruction);
            }
        }
    }
}

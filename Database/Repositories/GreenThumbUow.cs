namespace GreenThumb.Database.Repositories
{
    public class GreenThumbUow
    {
        private AppDbContext _context;
        public GardenModelRepository GardenRepo { get; }
        public GardenPlantRepository GardenPlantRepo { get; }
        public InstructionModelRepository InstructionRepo { get; }
        public PlantModelRepository PlantRepo { get; }
        public UserModelRepository UserRepo { get; }

        public GreenThumbUow(AppDbContext context)
        {
            _context = context;
            GardenRepo = new(context);
            GardenPlantRepo = new(context);
            InstructionRepo = new(context);
            PlantRepo = new(context);
            UserRepo = new(context);
        }

        async public Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

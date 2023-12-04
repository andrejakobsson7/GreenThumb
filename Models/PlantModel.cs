using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    public class PlantModel
    {
        [Key]
        [Column("id")]
        public int PlantId { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("plant_date")]
        public DateTime PlantDate { get; set; }

        public List<InstructionModel> Instructions { get; set; } = new();
        public List<GardenPlant> GardenPlants { get; set; } = new();

    }
}

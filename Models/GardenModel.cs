using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    public class GardenModel
    {
        [Key]
        [Column("id")]
        public int GardenId { get; set; }

        [Column("garden_name")]
        public string GardenName { get; set; } = null!;

        [Column("user_id")]
        public int UserId { get; set; }

        public UserModel User { get; set; } = null!;
        public List<GardenPlant> GardenPlants { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;

namespace InternationalGarage2_0.Models
{
    public class VehicleType
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // navigation collection 
//        public ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}
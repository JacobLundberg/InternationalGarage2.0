using System;
using System.ComponentModel.DataAnnotations;

namespace InternationalGarage2_0.Models
{

    public class ParkedVehicle
    {
        [Key,Required]
        public int Id { get; set; }

        public int/*VehicleType2*/ Type { get; set; }

        // foreign key 
        public int VehicleTypeId { get; set; }
        // navigation reference
        public VehicleType VehicleType { get; set; }

        // foreign key 
        public int MemberId { get; set; }
        // navigation reference
        public Member Member { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9]{3,5}$", ErrorMessage = "Invalid license numbers.")]
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }

        [Display(Name="Parked Time")]
        public DateTime TimeStampCheckIn { get; set; }
        public DateTime? TimeStampCheckOut { get; set; }

        public ParkedVehicle()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Models
{

    public class ParkedVehicle
    {
        [Key,Required]
        public int Id { get; set; }

        public VehicleType Type { get; set; }

        [RegularExpression(@"^[A-Z]{3}\d{3}$")]
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        [Display(Name ="Parked Time")]
        public DateTime TimeStampCheckIn { get; set; }
        public DateTime? TimeStampCheckOut { get; set; }

        public ParkedVehicle()
        {
        }
    }
}

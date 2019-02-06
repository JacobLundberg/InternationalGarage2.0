using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> Types { get; set; }
        public VehicleType2 Type { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9]{3,5}$", ErrorMessage = "Invalid license numbers.")]
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool ShowErrorMessage => ErrorMessage.Length > 0 ? true : false;
    }
}

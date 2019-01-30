using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> Types { get; set; }
        public VehicleType Type { get; set; }
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool ShowErrorMessage => ErrorMessage.Length > 0 ? true : false;
    }
}

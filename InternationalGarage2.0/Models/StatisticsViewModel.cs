using System;
using System.ComponentModel.DataAnnotations;
using InternationalGarage2_0.BLL;

namespace InternationalGarage2_0.Models
{
    public class StatisticsViewModel
    {
        [Display(Name = "Number Of Wheels")]
        public int TotalNumberOfWheels { get; set; }

        [Display(Name = "Vehicles")]
        public int TotalNumberOfParkedVehicles { get; set; }

        public double CurrentSumMinutes { get; set; }

        [Display(Name = "Fees")]
        public string CurrentSumParkingFeesDisplay { get; set; }

        public StatisticsViewModel()
        {
            
        }
    }
}

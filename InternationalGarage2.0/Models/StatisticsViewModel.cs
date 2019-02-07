using System;
using System.ComponentModel.DataAnnotations;

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
        public string CurrentSumParkingFeesDisplay => CurrentSumMinutes.ToString("C") + $" (Minutes: {Math.Round(CurrentSumMinutes, 2)}) ";

        public StatisticsViewModel()
        {
            TotalNumberOfWheels = 1;

            TotalNumberOfParkedVehicles = 2;

            CurrentSumMinutes = 3;
        }
    }
}

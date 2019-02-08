using InternationalGarage2_0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using InternationalGarage2_0.BLL;

namespace InternationalGarage2_0.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly InternationalGarage2_0Context _context;

        public StatisticsController(InternationalGarage2_0Context context)
        {
            _context = context;
        }

        // GET: Statistics
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.ParkedVehicle.ToListAsync();
            
            var currentParkedVehicles = vehicles.Where(a => a.TimeStampCheckOut == null).ToList();

            double totalCurrentParkedVehicleMinutes = 0;
            int totalNumberOfWheels = 0;
            var currentTime = DateTime.Now;
            currentParkedVehicles.ForEach(a => {
                totalNumberOfWheels += a.NumberOfWheels;
                var timeSpan = currentTime - a.TimeStampCheckIn;
                totalCurrentParkedVehicleMinutes += timeSpan.TotalMinutes;
            });

            var statisticsModel = new StatisticsViewModel() {
                TotalNumberOfParkedVehicles = currentParkedVehicles.Count,
                TotalNumberOfWheels = totalNumberOfWheels,
                CurrentSumMinutes = totalCurrentParkedVehicleMinutes,
                CurrentSumParkingFeesDisplay = ParkingFee.DisplayAsCurrency(totalCurrentParkedVehicleMinutes) + $" (Minutes: {Math.Round(totalCurrentParkedVehicleMinutes, 2)}) "
            };

            return View(statisticsModel);
        }
      
    }
}
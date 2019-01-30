using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternationalGarage2._0.Models;
using InternationalGarage2_0.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            double totalCurrentParkedVehicleFees = 0;
            int totalNumberOfWheels = 0;
            var currentTime = DateTime.Now;
            currentParkedVehicles.ForEach(a => {
                totalNumberOfWheels += a.NumberOfWheels;
                var timeSpan = currentTime - a.TimeStampCheckIn;
                totalCurrentParkedVehicleFees += timeSpan.TotalMinutes;
            });

            var statisticsModel = new StatisticsViewModel() {
                TotalNumberOfParkedVehicles = currentParkedVehicles.Count,
                TotalNumberOfWheels = totalNumberOfWheels,
                CurrentSumMinutes = totalCurrentParkedVehicleFees,
            };

            return View(statisticsModel);
        }
      
    }
}
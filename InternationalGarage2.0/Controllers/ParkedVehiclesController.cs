﻿using InternationalGarage2._0.Models;
using InternationalGarage2_0.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly InternationalGarage2_0Context _context;

        public ParkedVehiclesController(InternationalGarage2_0Context context)
        {
            _context = context;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && _context.ParkedVehicle.Count() == 0) Seed();  // If in development & db is empty -> seed some
        }

        /// <summary>
        /// Seed some mockup vehicles into database.
        /// </summary>
        protected void Seed()
        {
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Bus,
                    LicenseNumber = "BUZ666",
                    Color = "Red",
                    Model = "MAN",
                    NumberOfWheels = 6,
                    TimeStampCheckIn = new DateTime(2019, 01, 24, 22, 55, 21),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Car,
                    LicenseNumber = "KAR887",
                    Color = "Green",
                    Model = "Volvo",
                    NumberOfWheels = 4,
                    TimeStampCheckIn = new DateTime(2019, 01, 25, 19, 25, 11),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Car,
                    LicenseNumber = "CAB778",
                    Color = "Blue",
                    Model = "Saab",
                    NumberOfWheels = 4,
                    TimeStampCheckIn = new DateTime(2019, 01, 25, 19, 25, 11),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Motorcycle,
                    LicenseNumber = "MOT554",
                    Color = "Black",
                    Model = "Yamaha",
                    NumberOfWheels = 2,
                    TimeStampCheckIn = new DateTime(2019, 01, 26, 12, 44, 07),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Motorcycle,
                    LicenseNumber = "MOT554",
                    Color = "Black",
                    Model = "Yamaha",
                    NumberOfWheels = 2,
                    TimeStampCheckIn = new DateTime(2019, 01, 26, 12, 44, 07),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Motorcycle,
                    LicenseNumber = "MCC221",
                    Color = "Red",
                    Model = "Husqvarna",
                    NumberOfWheels = 2,
                    TimeStampCheckIn = new DateTime(2018, 01, 22, 23, 14, 57),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.RV,
                    LicenseNumber = "MOT554",
                    Color = "Cream White",
                    Model = "Hymer",
                    NumberOfWheels = 6,
                    TimeStampCheckIn = new DateTime(2019, 01, 24, 08, 03, 44),
                    TimeStampCheckOut = null
                });
            _context.ParkedVehicle.Add(
                new ParkedVehicle()
                {
                    Type = VehicleType.Truck,
                    LicenseNumber = "JLA987",
                    Color = "Silver",
                    Model = "Scania",
                    NumberOfWheels = 18,
                    TimeStampCheckIn = new DateTime(2019, 01, 22, 11, 24, 57),
                    TimeStampCheckOut = null
                });
            _context.SaveChanges();
        }

        public async Task<IActionResult> SearchVehicleLicenseNumber(string licenseNumber)
        {
            licenseNumber = licenseNumber.ToUpper();
            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.LicenseNumber == licenseNumber);
            if (parkedVehicle == null)
            {
                var dummyVehicle = new ParkedVehicle { Id = -1, LicenseNumber = licenseNumber };  // Ugly solution for view to recognize the license was not found!
                return View(dummyVehicle);
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkedVehicle.ToListAsync());
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels,TimeStampCheckIn,TimeStampCheckOut")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels,TimeStampCheckIn,TimeStampCheckOut")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            _context.ParkedVehicle.Remove(parkedVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }
    }
}

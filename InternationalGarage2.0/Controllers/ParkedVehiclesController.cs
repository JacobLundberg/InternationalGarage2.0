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
                    LicenseNumber = "TOM554",
                    Color = "Silver",
                    Model = "Honda",
                    NumberOfWheels = 2,
                    TimeStampCheckIn = new DateTime(2019, 01, 20, 12, 22, 07),
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
                    LicenseNumber = "DDS154",
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
            //Rewrite this func for checkout operations.
            var context2 = from veh in _context.ParkedVehicle where veh.TimeStampCheckOut == null select veh;
            return View(await context2.ToListAsync());
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

            var model = new EditViewModel() {
                Color = parkedVehicle.Color,
                Id = parkedVehicle.Id,
                LicenseNumber = parkedVehicle.LicenseNumber,
                Model = parkedVehicle.Model,
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                Type = parkedVehicle.Type,
                Types = GetTypes()
            };
            
            return View(model);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels")] EditViewModel parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    parkedVehicle.Types = GetTypes();
                    var vehicle = _context.ParkedVehicle.FirstOrDefault(a => a.Id == id);
                    if(vehicle == null)
                    {
                        parkedVehicle.ErrorMessage = $"Could not find vehicle with id {id}";
                        return View(parkedVehicle);
                    }

                    if(vehicle.LicenseNumber != parkedVehicle.LicenseNumber)
                    {
                        if(IsLicenceNumberCheckedIn(parkedVehicle.LicenseNumber))
                        {
                            parkedVehicle.ErrorMessage = GetLicenseAlreadyParkedErrorMsg(parkedVehicle.LicenseNumber);
                            return View(parkedVehicle);
                        }
                    }

                    vehicle.LicenseNumber = parkedVehicle.LicenseNumber;
                    vehicle.Model = parkedVehicle.Model;
                    vehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;
                    vehicle.Type = parkedVehicle.Type;
                    vehicle.Color = parkedVehicle.Color;

                    _context.Update(vehicle);
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
            //_context.ParkedVehicle.Remove(parkedVehicle);
            parkedVehicle.TimeStampCheckOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Receipt),parkedVehicle);
        }

        // GET: ParkedVehicles/Check In
        public IActionResult CheckIn()
        {
            var res = new CheckInViewModel() { Types = GetTypes() };
            return View(res);
        }

        // POST: ParkedVehicles/CheckIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn([Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels")] CheckInViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                var parkedVehicle = new ParkedVehicle
                {
                    Id = vehicle.Id,
                    Color = vehicle.Color,
                    LicenseNumber = vehicle.LicenseNumber,
                    Model = vehicle.Model,
                    Type = vehicle.Type,
                    NumberOfWheels = vehicle.NumberOfWheels,
                    TimeStampCheckIn = DateTime.Now
                };

                if (IsLicenceNumberCheckedIn(vehicle.LicenseNumber))
                {
                    vehicle.Types = GetTypes();
                    vehicle.ErrorMessage = GetLicenseAlreadyParkedErrorMsg(vehicle.LicenseNumber);
                    return View(vehicle);
                }

                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetTypes()
        {
            var res = new List<SelectListItem>();
            var values = Enum.GetValues(typeof(VehicleType));
            foreach (var item in values)
            {
                var text = item.ToString();
                res.Add(new SelectListItem(text, text));
            }
            return res;
        }

        private bool IsLicenceNumberCheckedIn(string licenseNumber)
        {
            var parkedVehicle = _context.ParkedVehicle
                .FirstOrDefault(m => m.LicenseNumber == licenseNumber);
            if (parkedVehicle != null)
            {
                return true;
            }
            return false;
        }

        public IActionResult Receipt(ParkedVehicle vehout)
        {
            var tin = vehout.TimeStampCheckIn;
            var tout = vehout.TimeStampCheckOut ?? DateTime.Now;
            Receipt prReceipt = new Receipt
            {
                LicenseNumber = vehout.LicenseNumber,
                Type = vehout.Type.ToString(),
                Color = vehout.Color,
                Model = vehout.Model,
                NumberOfWheels = vehout.NumberOfWheels,
                TimeStampCheckIn = tin,
                TimeStampCheckOut = tout,
                Cash = (int)(tout - tin).TotalMinutes
            };
            return View(prReceipt);
        }

        private string GetLicenseAlreadyParkedErrorMsg(string licenseNumber)
        {
            return $"Vehicle with License {licenseNumber} already parked";
        }

    }
}

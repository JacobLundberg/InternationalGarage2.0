﻿using InternationalGarage2_0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternationalGarage2_0.BLL;

namespace InternationalGarage2_0.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly InternationalGarage2_0Context _context;

        public ParkedVehiclesController(InternationalGarage2_0Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search2_5(string searchString)
        {
            List<ParkedVehicle> parkedVehicle = await _context.ParkedVehicle
                .Include(v => v.VehicleType)
                .Include(m => m.Member)
                .Where(p => p.LicenseNumber.Contains(searchString) || (p.VehicleType.Name == searchString))
                .ToListAsync();
            ViewBag.Search2_5 = searchString;
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> ListGarage(string sortBy, string sortAsc = null)
        {
            return View(await GetSortedVehicles(sortBy, sortAsc));
        }

        // GET: ParkedVehicles
        public /*async Task<*/IActionResult/*>*/ Index(/*string sortBy = null*/)
        {
            return View(/*await GetSortedVehicles(sortBy)*/);
        }

        private async Task<List<ParkedVehicle>> GetSortedVehicles(string sortBy, string sortAsc)
        {
            var doSortAsc = false;
            ViewBag.sortAsc = sortBy;
            if (!string.IsNullOrEmpty(sortAsc) && sortBy == sortAsc)
            {
                doSortAsc = true;
                ViewBag.sortAsc = string.Empty;
            }

            var vehicleSelection = _context.ParkedVehicle
                .Include(m => m.Member)
                .Include(t => t.VehicleType)
                .Where(v => v.TimeStampCheckOut == null);


            if (sortBy == "Member")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.Member.Name);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.Member.Name);
                }
            }
            if (sortBy == "VehicleType")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.VehicleType.Name);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.VehicleType.Name);
                }


            }
            else if (sortBy == "Color")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.Color);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.Color);
                }

            }
            else if (sortBy == "TimeStampCheckIn")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.TimeStampCheckIn);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.TimeStampCheckIn);
                }
            }
            else if (sortBy == "NumberOfWheels")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.NumberOfWheels);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.NumberOfWheels);
                }

            }
            else if (sortBy == "Model")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.Model);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.Model);
                }
            }
            else if (sortBy == "LicenseNumber")
            {
                if (doSortAsc)
                {
                    vehicleSelection = vehicleSelection.OrderBy(a => a.LicenseNumber);
                }
                else
                {
                    vehicleSelection = vehicleSelection.OrderByDescending(a => a.LicenseNumber);
                }

            }

            return await vehicleSelection.ToListAsync();
        }


        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicles = _context.ParkedVehicle
                .Include(a => a.VehicleType)
                .Include(b=> b.Member);

            var parkedVehicle = await parkedVehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
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

            var parkedVehicles = _context.ParkedVehicle
                .Include(a => a.Member)
                .Include(b => b.VehicleType);

            var parkedVehicle = await parkedVehicles.FirstOrDefaultAsync(c => c.Id == id);

            if (parkedVehicle == null)
            {
                return NotFound();
            }

            var model = new EditViewModel()
            {
                Color = parkedVehicle.Color,
                Id = parkedVehicle.Id,
                LicenseNumber = parkedVehicle.LicenseNumber,
                Model = parkedVehicle.Model,
                NumberOfWheels = parkedVehicle.NumberOfWheels,
                Type = parkedVehicle.VehicleType.Id,
                Types = await GetTypesAsync(),
                MemberId = parkedVehicle.MemberId,
                Members = await GetMembersAsync()
            };

            return View(model);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels, MemberId")] EditViewModel parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    parkedVehicle.Types = await GetTypesAsync();
                    var vehicle = _context.ParkedVehicle.FirstOrDefault(a => a.Id == id);
                    if (vehicle == null)
                    {
                        parkedVehicle.ErrorMessage = $"Could not find vehicle with id {id}";
                        return View(parkedVehicle);
                    }

                    if (vehicle.LicenseNumber != parkedVehicle.LicenseNumber)
                    {
                        if (IsLicenceNumberCheckedIn(parkedVehicle.LicenseNumber))
                        {
                            parkedVehicle.ErrorMessage = GetLicenseAlreadyParkedErrorMsg(parkedVehicle.LicenseNumber);
                            return View(parkedVehicle);
                        }
                    }

                    vehicle.LicenseNumber = parkedVehicle.LicenseNumber;
                    vehicle.Model = parkedVehicle.Model;
                    vehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;

                    vehicle.VehicleTypeId = parkedVehicle.Type;
                    vehicle.MemberId = parkedVehicle.MemberId;

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
                return RedirectToAction(nameof(ListGarage));
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

            var parkedVehicles = _context.ParkedVehicle
                .Include(b => b.Member)
                .Include(a => a.VehicleType);

            var parkedVehicle = await parkedVehicles
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
            var parkedVehicles = _context.ParkedVehicle
                .Include(a => a.VehicleType);
            var parkedVehicle = await parkedVehicles.FirstOrDefaultAsync(a => a.Id == id);
            //_context.ParkedVehicle.Remove(parkedVehicle);
            parkedVehicle.TimeStampCheckOut = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Receipt), parkedVehicle );
        }

        // GET: ParkedVehicles/Check In
        public async Task<IActionResult> CheckIn()
        {
            var res = new CheckInViewModel()
            {
                Types = await GetTypesAsync(),
                Members = await GetMembersAsync()
            };
            return View(res);
        }

        // POST: ParkedVehicles/CheckIn
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn([Bind("Id,Type,LicenseNumber,Color,Model,NumberOfWheels, MemberID")] CheckInViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                var parkedVehicle = new ParkedVehicle
                {
                    Id = vehicle.Id,
                    Color = vehicle.Color,
                    LicenseNumber = vehicle.LicenseNumber,
                    Model = vehicle.Model,
                  
                    NumberOfWheels = vehicle.NumberOfWheels,
                    TimeStampCheckIn = DateTime.Now,

                    MemberId = vehicle.MemberID,
                    VehicleTypeId = vehicle.Type
                };

                if (IsLicenceNumberCheckedIn(vehicle.LicenseNumber))
                {
                    vehicle.Types = await GetTypesAsync();
                    vehicle.ErrorMessage = GetLicenseAlreadyParkedErrorMsg(vehicle.LicenseNumber);
                    return View(vehicle);
                }

                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListGarage));
            }
            return View(vehicle);
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        private async Task<List<SelectListItem>> GetTypesAsync()
        {
            var vehicleTypes = await _context.VehicleType.ToListAsync();
            var res = new List<SelectListItem>();
            foreach (var item in vehicleTypes)
            {
                var text = item.Name;
                res.Add(new SelectListItem(text, item.Id.ToString()));
            }
            return res;
        }

        private async Task<List<SelectListItem>> GetMembersAsync()
        {
            var members = await _context.Member.ToListAsync();
            var res = new List<SelectListItem>();
            foreach (var item in members)
            {
                var text = item.Name;
                res.Add(new SelectListItem(text, item.Id.ToString()));
            }
            return res;
        }

        private bool IsLicenceNumberCheckedIn(string licenseNumber)
        {
            var parkedVehicle = _context.ParkedVehicle
                .Where(a => a.TimeStampCheckOut == null)
                .FirstOrDefault(b => b.LicenseNumber == licenseNumber);

            if (parkedVehicle != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IActionResult> Receipt(ParkedVehicle vehout)
        {
            var vehicleType = await _context.VehicleType.FirstOrDefaultAsync(a => a.Id == vehout.VehicleTypeId);
            var member = await _context.Member.FirstOrDefaultAsync(a => a.Id == vehout.MemberId);

            var tin = vehout.TimeStampCheckIn;
            var tout = vehout.TimeStampCheckOut ?? DateTime.Now;
            Receipt prReceipt = new Receipt
            {
                MemberName = member.Name,
                LicenseNumber = vehout.LicenseNumber,
                Type = vehicleType.Name,
                Color = vehout.Color,
                Model = vehout.Model,
                NumberOfWheels = vehout.NumberOfWheels,
                TimeStampCheckIn = tin,
                TimeStampCheckOut = tout,
                //Cash = Math.Round((tout - tin).TotalMinutes),
                FeeDisplay = ParkingFee.DisplayAsCurrency(tin, tout)
            };
            return View(prReceipt);
        }

        private string GetLicenseAlreadyParkedErrorMsg(string licenseNumber)
        {
            return $"Vehicle with License {licenseNumber} already parked";
        }

    }
}

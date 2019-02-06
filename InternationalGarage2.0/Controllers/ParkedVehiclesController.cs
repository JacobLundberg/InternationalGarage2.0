using InternationalGarage2._0.Models;
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
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                SeedMember();  // If in development & db is empty -> seed some
                SeedVehicleType();
                SeedParkedVehice();  // If in development & db is empty -> seed some
            }
        }

        /// <summary>
        /// Add data seed into members table only if it has no data.
        /// </summary>
        private void SeedMember()
        {
            if (_context.Member.Count() != 0) return;

            var people = new List<Member>();
            people.Add(new Member { Name = "Cooper Botsford" });
            people.Add(new Member { Name = "Kaelyn Christiansen" });
            people.Add(new Member { Name = "Felipe Pacocha" });
            people.Add(new Member { Name = "Sherwood Will" });
            people.Add(new Member { Name = "Carlos Reynolds" });
            _context.Member.AddRange(people);
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed some mockup vehicleTypes into database.
        /// </summary>
        private void SeedVehicleType()
        {
            if (_context.VehicleType.Count() != 0) return;
            var vehicles = new List<VehicleType>();
            vehicles.Add(new VehicleType { Name = "Bus" });
            vehicles.Add(new VehicleType { Name = "Car" });
            vehicles.Add(new VehicleType { Name = "Motorcycle" });
            vehicles.Add(new VehicleType { Name = "RV" });
            vehicles.Add(new VehicleType { Name = "Truck" });
            _context.VehicleType.AddRange(vehicles);
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed some mockup vehicles into database.
        /// </summary>
        protected void SeedParkedVehice()
        {
            if (_context.ParkedVehicle.Count() != 0) return;
            var member = _context.Member.FirstOrDefault();
            var vehicle = _context.VehicleType.FirstOrDefault();
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = VehicleType2.Car,
                LicenseNumber = "BUZ987",
                Color = "Red",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 01, 24, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            });
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = VehicleType2.Bus,
                LicenseNumber = "BUZ666",
                Color = "Red",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 01, 24, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            });
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = VehicleType2.Bus,
                LicenseNumber = "XXX666",
                Color = "Grey",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 02, 14, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            }); _context.SaveChanges();
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

        public async Task<IActionResult> Search2_5(string searchString)
        {
            List<ParkedVehicle> parkedVehicle = _context.ParkedVehicle
                .Include(m => m.VehicleType)
                .Where(m => m.LicenseNumber.Contains(searchString) || (m.VehicleType.Name == searchString))
                .ToList();
            ViewBag.Search2_5 = searchString;
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> ListGarage(string sortBy)
        {
            if (sortBy != null)
            {
                return View(await GetSortedVehicles(sortBy));
            }
            //Rewrite this func for checkout operations.
            var context2 = from veh in _context.ParkedVehicle where veh.TimeStampCheckOut == null select veh;
            return View(await context2.ToListAsync());
        }

        // GET: ParkedVehicles
        public async Task<IActionResult> Index(string sortBy = null)
        {
            if (sortBy != null)
            {
                return View(await GetSortedVehicles(sortBy));
            }
            //Rewrite this func for checkout operations.
            var context2 = from veh in _context.ParkedVehicle where veh.TimeStampCheckOut == null select veh;
            return View(await context2.ToListAsync());
        }

        private async Task<List<ParkedVehicle>> GetSortedVehicles(string sortBy)
        {
            if (sortBy == "Type")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.Type).ToListAsync();
            }
            else if (sortBy == "Color")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.Color).ToListAsync();
            }
            else if (sortBy == "TimeStampCheckIn")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.TimeStampCheckIn).ToListAsync();
            }
            else if (sortBy == "NumberOfWheels")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.NumberOfWheels).ToListAsync();
            }
            else if (sortBy == "Model")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.Model).ToListAsync();
            }
            else if (sortBy == "LicenseNumber")
            {
                return await _context.ParkedVehicle.Where(a => a.TimeStampCheckOut == null).OrderBy(a => a.LicenseNumber).ToListAsync();
            }

            return await _context.ParkedVehicle.ToListAsync();
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

            var model = new EditViewModel()
            {
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
            return RedirectToAction(nameof(Receipt), parkedVehicle);
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
                    vehicle.Types = GetTypes();
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

        private List<SelectListItem> GetTypes()
        {
            var res = new List<SelectListItem>();
            var values = Enum.GetValues(typeof(VehicleType2));
            foreach (var item in values)
            {
                var text = item.ToString();
                res.Add(new SelectListItem(text, text));
            }
            return res;
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
                Cash = Math.Round((tout - tin).TotalMinutes)
            };
            return View(prReceipt);
        }

        private string GetLicenseAlreadyParkedErrorMsg(string licenseNumber)
        {
            return $"Vehicle with License {licenseNumber} already parked";
        }

    }
}

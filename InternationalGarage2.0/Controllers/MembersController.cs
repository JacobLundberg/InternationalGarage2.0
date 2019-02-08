using InternationalGarage2_0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Controllers
{
    public class MembersController : Controller
    {
        private readonly InternationalGarage2_0Context _context;

        private class MemVehicle
        {
            public int mID { get; set; }
            public int mVehicleNum { get; set; }
        };

        public MembersController(InternationalGarage2_0Context context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            IQueryable<MemVehicle> numVeh = from vh in _context.ParkedVehicle
                                            group vh by vh.MemberId into numGp
                                            select new MemVehicle() { mID = numGp.Key, mVehicleNum = numGp.Count() };
            IDictionary<int, MemVehicle> vehiclesSearched;
            vehiclesSearched = await numVeh.AsNoTracking().ToDictionaryAsync(x => x.mID, x => x);
            foreach (var item in _context.Member)
            {
                if (vehiclesSearched.ContainsKey(item.Id))
                {
                    item.NumVehicles = vehiclesSearched[item.Id].mVehicleNum;
                }
                else
                {
                    item.NumVehicles = 0;
                }
            }

            return View(await _context.Member.ToListAsync());
        }

        // GET: Search
        public IActionResult Search()
        {
            var search = new SearchMember
            {
                SearchResult = new List<Member>()
            };
            return View("SearchMember", search);
        }


        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("NameSearch")] SearchMember member)
        {
            if (ModelState.IsValid)
            {
                var searchString = member.NameSearch;
                var res = _context.Member.Where(a => a.Name.IndexOf(searchString) > -1);
                member.SearchResult = await res.ToListAsync();
                return View(nameof(SearchMember), member);
            }
            return NotFound();

        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.Id == id);
        }
    }
}

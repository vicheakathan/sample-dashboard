using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dashboard.Models;
using Sample.Data;

namespace Dashboard.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly SampleContext _context;

        public CompaniesController(SampleContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string search)
        {
            //return _context.sma_companies != null ? 
            //            View(await _context.sma_companies.ToListAsync()) :
            //            Problem("Entity set 'AppDbContext.sma_companies'  is null.");

            var appDbContext = _context.sma_companies.OrderByDescending(x => x.id);

            if (!String.IsNullOrEmpty(search))
            {
                appDbContext = appDbContext
                        .Where(a => a.name!.Contains(search.ToLower()) || a.company_name!.Contains(search.ToLower()) || a.email!.Contains(search.ToLower()))
                        .OrderByDescending(p => p.id);
            }

            return View(await appDbContext.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sma_companies == null)
            {
                return NotFound();
            }

            var company = await _context.sma_companies
                .FirstOrDefaultAsync(m => m.id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,company_name,phone,email,address,logo")] Company company)
        {
            if (ModelState.IsValid)
            {
                TempData["AlertMessageSuccess"] = "Customer created successful.";
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sma_companies == null)
            {
                return NotFound();
            }

            var company = await _context.sma_companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,company_name,phone,email,address,logo")] Company company)
        {
            if (id != company.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["AlertMessageSuccess"] = "Customer updated successful.";
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sma_companies == null)
            {
                return NotFound();
            }

            var company = await _context.sma_companies
                .FirstOrDefaultAsync(m => m.id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sma_companies == null)
            {
                return Problem("Entity set 'AppDbContext.sma_companies'  is null.");
            }
            var company = await _context.sma_companies.FindAsync(id);
            if (company != null)
            {
                TempData["AlertMessageSuccess"] = "Customer deleted successful.";
                _context.sma_companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return (_context.sma_companies?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

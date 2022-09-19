using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Dashboard.Models;

namespace Dashboard.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly SampleContext _context;

        public CategoriesController(SampleContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string search)
        {
            var appDbContext = _context.sma_categories.OrderByDescending(x => x.id);

            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.keysearch = search;
                appDbContext = appDbContext
                                .Where(p => p.name!.Contains(search.ToLower()))
                                .OrderByDescending(x => x.id);
            }

            return View(await appDbContext.ToArrayAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sma_categories == null)
            {
                return NotFound();
            }

            var category = await _context.sma_categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name")] Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["AlertMessageSuccess"] = "Category created successful.";
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sma_categories == null)
            {
                return NotFound();
            }

            var category = await _context.sma_categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name")] Category category)
        {
            if (id != category.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["AlertMessageSuccess"] = "Category updated successful.";
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sma_categories == null)
            {
                return NotFound();
            }

            var category = await _context.sma_categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sma_categories == null)
            {
                return Problem("Entity set 'AppDbContext.sma_categories'  is null.");
            }

            var categories = _context.sma_categories.FirstOrDefault(m => m.id == id);
            var product = _context.sma_products.FirstOrDefault(m => m.category_id == id);
            var expense = _context.sma_expenses.FirstOrDefault(m => m.category_id == id);

            var category = await _context.sma_categories.FindAsync(id);
            if (category != null)
            {
                if (product != null || expense != null)
                {
                    TempData["AlertMessageError"] = "The category <strong>" + category.name + "</strong> can not delete.";
                }
                else
                {
                    TempData["AlertMessageSuccess"] = "Category deleted successful.";
                    _context.sma_categories.Remove(category);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.sma_categories?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

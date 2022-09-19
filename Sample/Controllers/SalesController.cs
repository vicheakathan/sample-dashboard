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
    public class SalesController : Controller
    {
        private readonly SampleContext _context;

        public SalesController(SampleContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index(string search)
        {
            //var appDbContext = _context.sma_sales
            //                    .Include(s => s.biller_)
            //                    .Include(s => s.customer_)
            //                    .OrderByDescending(s => s.id);
            //return View(await appDbContext.ToListAsync());


            var appDbContext = _context.sma_sales
                                .Include(s => s.biller_)
                                .Include(s => s.customer_)
                                .OrderByDescending(s => s.id);
            if (!string.IsNullOrEmpty(search))
            {
                appDbContext = appDbContext.Where(s => s.customer_.name!.Contains(search.ToLower()))
                                .Include(s => s.biller_)
                                .Include(s => s.customer_)
                                .OrderByDescending(s => s.id);
            }
            ViewBag.keysearch = search;

            return View(await appDbContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sma_sales == null)
            {
                return NotFound();
            }

            var sale = await _context.sma_sales
                .Include(s => s.biller_)
                .Include(s => s.customer_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["biller_id"] = new SelectList(_context.sma_users, "id", "username");
            ViewData["customer_id"] = new SelectList(_context.sma_companies, "id", "name");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,date,reference_no,total,discount,shipping,grand_total,sale_status,payment_status,payment_method,customer_id,biller_id")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                TempData["AlertMessageSuccess"] = "Sale created successful.";
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["biller_id"] = new SelectList(_context.sma_users, "id", "username", sale.biller_id);
            ViewData["customer_id"] = new SelectList(_context.sma_companies, "id", "name", sale.customer_id);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sma_sales == null)
            {
                return NotFound();
            }

            var sale = await _context.sma_sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            ViewData["biller_id"] = new SelectList(_context.sma_users, "id", "username", sale.biller_id);
            ViewData["customer_id"] = new SelectList(_context.sma_companies, "id", "name", sale.customer_id);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,date,reference_no,total,discount,shipping,grand_total,sale_status,payment_status,payment_method,customer_id,biller_id")] Sale sale)
        {
            if (id != sale.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["AlertMessageSuccess"] = "Sale updated successfull.";
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.id))
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
            ViewData["biller_id"] = new SelectList(_context.sma_users, "id", "username", sale.biller_id);
            ViewData["customer_id"] = new SelectList(_context.sma_companies, "id", "name", sale.customer_id);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sma_sales == null)
            {
                return NotFound();
            }

            var sale = await _context.sma_sales
                .Include(s => s.biller_)
                .Include(s => s.customer_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sma_sales == null)
            {
                return Problem("Entity set 'AppDbContext.sma_sales'  is null.");
            }
            var sale = await _context.sma_sales.FindAsync(id);
            if (sale != null)
            {
                TempData["AlertMessageSuccess"] = "Sale deleted successful.";
                _context.sma_sales.Remove(sale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
          return (_context.sma_sales?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

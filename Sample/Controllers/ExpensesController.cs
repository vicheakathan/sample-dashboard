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
    public class ExpensesController : Controller
    {
        private readonly SampleContext _context;

        public ExpensesController(SampleContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.sma_expenses.Include(e => e.category_);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sma_expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.sma_expenses
                .Include(e => e.category_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,amount,date,description,category_id")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                TempData["AlertMessageSuccess"] = "Expense created successful.";
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", expense.category_id);
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sma_expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.sma_expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", expense.category_id);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,amount,date,description,category_id")] Expense expense)
        {
            if (id != expense.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["AlertMessageSuccess"] = "Expense updated successful.";
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.id))
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
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", expense.category_id);
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sma_expenses == null)
            {
                return NotFound();
            }

            var expense = await _context.sma_expenses
                .Include(e => e.category_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sma_expenses == null)
            {
                return Problem("Entity set 'AppDbContext.sma_expenses'  is null.");
            }
            var expense = await _context.sma_expenses.FindAsync(id);
            if (expense != null)
            {
                TempData["AlertMessageSuccess"] = "Expense deleted successful.";
                _context.sma_expenses.Remove(expense);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
          return (_context.sma_expenses?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

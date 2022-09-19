using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Data;

namespace Sample.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public readonly SampleContext _context;
        public DashboardController(SampleContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //DateTime StartDate = DateTime.Today.AddDays(-30);
            //DateTime EndDate = DateTime.Today;

            DateTime now = DateTime.Now;
            var StartDate = new DateTime(now.Year, now.Month, 1);
            var EndDate = StartDate.AddMonths(1).AddDays(-1);

            // total expense
            List<Expense> expenses = await _context.sma_expenses
                                    .Include(x => x.category_)
                                    .Where(y => y.date >= StartDate && y.date <= EndDate)
                                    .ToListAsync();
            double total_expense = expenses.Sum(e => e.amount);
            ViewBag.TotalExpense = String.Format("{0:C}", total_expense);


            // total sale
            List<Sale> sale = await _context.sma_sales
                                   .Include(x => x.customer_)
                                   .Include(x1 => x1.biller_)
                                   .Where(y => y.date >= StartDate && y.date <= EndDate)
                                   .ToListAsync();
            double total_sale = sale.Sum(s => s.grand_total);
            ViewBag.TotalSale = String.Format("{0:C}", total_sale);

            return View();
        }
    }
}

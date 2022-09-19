using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dashboard.Models;
using Sample.Data;

namespace Dashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SampleContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(SampleContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _env = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(string search)
        {
            //var appDbContext = _context.sma_products
            //                    .Include(p => p.category_)
            //                    .OrderByDescending(p => p.id);
            //return View(await appDbContext.ToListAsync());

            var appDbContext = _context.sma_products
                                .Include(p => p.category_)
                                .OrderByDescending(p => p.id);

            if (!String.IsNullOrEmpty(search))
            {
                appDbContext = appDbContext
                        .Where(s => s.name!.Contains(search.ToLower()) || s.barcode!.Contains(search.ToLower()) || s.category_.name!.Contains(search.ToLower()))
                        .Include(p => p.category_)
                        .OrderByDescending(p => p.id);
            }

            ViewBag.keysearch = search;
            return View(await appDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sma_products == null)
            {
                return NotFound();
            }

            var product = await _context.sma_products
                .Include(p => p.category_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Products/Create
        public IActionResult Create()
        {
            //TempData["AlertMessage"] = "Product Created Successfull.";
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,barcode,name,cost,price,image,category_id")] Product product, List<IFormFile> postfileImage)
        {
            foreach (var file in postfileImage)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\images\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                TempData["AlertMessageSuccess"] = "Product created successfull.";
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", product.category_id);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sma_products == null)
            {
                return NotFound();
            }

            var product = await _context.sma_products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", product.category_id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_bk(int id, [Bind("id,barcode,name,cost,price,image,category_id")] Product product)
        {
            if (id != product.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
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


            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", product.category_id);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sma_products == null)
            {
                return NotFound();
            }

            var product = await _context.sma_products
                .Include(p => p.category_)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sma_products == null)
            {
                return Problem("Entity set 'AppDbContext.sma_products'  is null.");
            }
            var product = await _context.sma_products.FindAsync(id);
            if (product != null)
            {
                TempData["AlertMessageSuccess"] = "Product deleted successfull.";
                _context.sma_products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.sma_products?.Any(e => e.id == id)).GetValueOrDefault();
        }


        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> image)
        {
            foreach (var file in image)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\images\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("id,barcode,name,image,cost,price,category_id")] Product product)
        {
            if (id != product.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //foreach (var file in fileImage)
                    //{
                    //    Random r = new Random();
                    //    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\images\\");
                    //    bool basePathExists = System.IO.Directory.Exists(basePath);
                    //    if (!basePathExists) Directory.CreateDirectory(basePath);
                    //    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    //    var filePath = Path.Combine(basePath, file.FileName);
                    //    var extension = Path.GetExtension(file.FileName);
                    //    if (!System.IO.File.Exists(filePath))
                    //    {
                    //        using (var stream = new FileStream(filePath, FileMode.Create))
                    //        {
                    //            await file.CopyToAsync(stream);
                    //        }
                    //    }
                    //}

                    //Guid guid = Guid.NewGuid();
                    //string newfileName = guid.ToString();
                    //string fileextention = Path.GetExtension(fileImage.FileName);
                    //string fileName = newfileName + fileextention;
                    //string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\images", fileName);
                    //var stream = new FileStream(uploadpath, FileMode.Create);
                    //fileImage.CopyToAsync(stream);

                    TempData["AlertMessageSuccess"] = "Product updated successfull.";
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
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

            //TempData["AlertMessage"] = "Product Updated Successfull.";

            ViewData["category_id"] = new SelectList(_context.sma_categories, "id", "name", product.category_id);
            return View(product);
        }
    }
}

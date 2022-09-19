using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Sample.Data;
using Sample.Models;
using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;


namespace Sample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SampleContext _context;

        public HomeController(ILogger<HomeController> logger, SampleContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //return View();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Student");
                var currentRow = 1;

                // header
                worksheet.Cell(currentRow, 1).Value = "ID";
                worksheet.Cell(currentRow, 2).Value = "Name";

                // body
                string[] tokens = { "170", "169", "168", "167" };
                int[] id = Array.ConvertAll<string, int>(tokens, int.Parse);
                var row = 2;

                for (int i = 0; i < id.Length; i++)
                {
                    var select_sale = _context.sma_sales.Where(a => a.id == id[i]);
                    if (select_sale != null)
                    {
                        foreach (var item in select_sale)
                        {
                            worksheet.Cell(row, 1).Value = item.id;
                            worksheet.Cell(row, 2).Value = "Than Vicheaka";
                            row++;
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var contect = stream.ToArray();
                    return File(
                        contect,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Students.xlsx"
                    );
                }
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult PageNotFound()
        {
            return View();
        }
        public async Task<ActionResult> APIUploadImage(IFormFile myfile)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                string newfileName = guid.ToString();
                string fileextention = Path.GetExtension(myfile.FileName);
                string fileName = newfileName + fileextention;

                var imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                imageExtensions.Add(".png");
                imageExtensions.Add(".jpg");
                imageExtensions.Add(".jpeg");
                string extension = Path.GetExtension(fileName);
                bool isImage = imageExtensions.Contains(extension);
                int fileError;
                if (isImage == true)
                {
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\images", fileName);
                    var stream = new FileStream(uploadpath, FileMode.Create);
                    myfile.CopyToAsync(stream);
                    fileError = 0;
                }
                else
                {
                    fileError = 1;
                }

                return Json(new { status = "success", filename = fileName, fileError = fileError });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error " + ex.Message });
            }
        }
        public async Task<ActionResult> APIActionDelete(string check_value)
        {
            //string tbl_name = "sma_" + table_name;
            int status = 0;
            string[] tokens = check_value.Split("-");
            int[] id = Array.ConvertAll<string, int>(tokens, int.Parse);
            if (check_value != "0")
            {
                for (int i = 0; i < id.Length; i++)
                {
                    if (id[i] > 0)
                    {
                        var obj = await _context.sma_sales.FindAsync(id[i]);
                        if (obj != null)
                        {
                            _context.Remove(obj);
                            status = 1;
                            TempData["AlertMessageSuccess"] = "Sale deleted successful.";
                        }
                    }
                }
                await _context.SaveChangesAsync();

                return Json(new { status = status });
            }
            else
                return Json(new { status = status });
        }
        public async Task<ActionResult> API_Bulk_Actions_bk(string check_value, string key_action)
        {
            //string tbl_name = "sma_" + table_name;
            string status = "0";
            string[] tokens = check_value.Split("-");
            int[] id = Array.ConvertAll<string, int>(tokens, int.Parse);
            var excel_file = "";
            var row = 2;
            var fileName = "";
            if (check_value != "0")
            {
                for (int i = 0; i < id.Length; i++)
                {
                    if (id[i] > 0)
                    {
                        if (key_action == "export_to_excel")
                        {
                            using (var workbook = new XLWorkbook())
                            {
                                var worksheet = workbook.Worksheets.Add("Sale");

                                // header
                                worksheet.Cell(1, 1).Value = "Date";
                                worksheet.Cell(1, 2).Value = "Reference No";
                                worksheet.Cell(1, 3).Value = "Customer";
                                worksheet.Cell(1, 4).Value = "Sale Status";
                                worksheet.Cell(1, 5).Value = "Grand Total";
                                worksheet.Cell(1, 6).Value = "Payment Status";
                                worksheet.Cell(1, 7).Value = "Paid By";

                                // body
                                var select_sale = _context.sma_sales.Where(a => a.id == id[i]);
                                if (select_sale != null)
                                {
                                    foreach (var item in select_sale)
                                    {
                                        worksheet.Cell(row, 1).Value = item.id;
                                        //worksheet.Cell(row, 2).Value = item.reference_no;
                                        //worksheet.Cell(row, 3).Value = item.customer_id;
                                        //worksheet.Cell(row, 4).Value = item.sale_status;
                                        //worksheet.Cell(row, 5).Value = item.grand_total;
                                        //worksheet.Cell(row, 6).Value = item.payment_status;
                                        //worksheet.Cell(row, 7).Value = item.payment_method;
                                        row++;
                                    }
                                }

                                using (var stream = new MemoryStream())
                                {
                                    workbook.SaveAs(stream);
                                    var contect = stream.ToArray();
                                    excel_file = Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length);
                                }
                                fileName = "Sale.xlsx";
                                status = "export_to_excel";
                            }
                        }
                        if (key_action == "export_to_pdf")
                        {

                        }
                    }
                }
                return Json(new { status = status, stream_file = excel_file, fileName = fileName });
            }
            else
                return Json(new { status = status });



            //using (var workbook = new XLWorkbook())
            //{
            //    var worksheet = workbook.Worksheets.Add("Student");

            //    // header
            //    worksheet.Cell(1, 1).Value = "Date";
            //    worksheet.Cell(1, 2).Value = "Reference No";
            //    worksheet.Cell(1, 3).Value = "Customer";
            //    worksheet.Cell(1, 4).Value = "Sale Status";
            //    worksheet.Cell(1, 5).Value = "Grand Total";
            //    worksheet.Cell(1, 6).Value = "Payment Status";
            //    worksheet.Cell(1, 7).Value = "Paid By";

            //    // body
            //    for (int i = 0; i < id.Length; i++)
            //    {
            //        var select_sale = _context.sma_sales.Where(a => a.id == id[i]);
            //        if (select_sale != null)
            //        {
            //            foreach (var item in select_sale)
            //            {
            //                worksheet.Cell(row, 1).Value = item.date;
            //                worksheet.Cell(row, 2).Value = item.reference_no;
            //                worksheet.Cell(row, 3).Value = item.customer_id;
            //                worksheet.Cell(row, 4).Value = item.sale_status;
            //                worksheet.Cell(row, 5).Value = item.grand_total;
            //                worksheet.Cell(row, 6).Value = item.payment_status;
            //                worksheet.Cell(row, 7).Value = item.payment_method;
            //                row++;
            //            }
            //        }
            //    }

            //    using (var stream = new MemoryStream())
            //    {
            //        workbook.SaveAs(stream);
            //        var contect = stream.ToArray();
            //        var fileName = "Sale.xlsx";

            //        return Json(new { status = "export_to_excel", result = contect, fileName = fileName });
            //    }
            //}

        }

        public async Task<ActionResult> API_Bulk_Actions(string check_value, string key_action)
        {
            string status = "0";
            string[] tokens = check_value.Split("-");
            int[] id = Array.ConvertAll<string, int>(tokens, int.Parse);
            var excel_file = "";
            var row = 2;
            var fileName = "";

            if (check_value != "0")
            {
                if (key_action == "export_to_excel")
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sale");

                        // header
                        worksheet.Cell(1, 1).Value = "Date";
                        worksheet.Cell(1, 2).Value = "Reference No";
                        worksheet.Cell(1, 3).Value = "Customer";
                        worksheet.Cell(1, 4).Value = "Sale Status";
                        worksheet.Cell(1, 5).Value = "Grand Total";
                        worksheet.Cell(1, 6).Value = "Payment Status";
                        worksheet.Cell(1, 7).Value = "Paid By";

                        // body
                        for (int i = 0; i < id.Length; i++)
                        {
                            var select_sale = _context.sma_sales
                                .Where(a => a.id == id[i])
                                .Include(c => c.customer_)
                                .OrderByDescending(a => a.date);

                            if (select_sale != null)
                            {
                                foreach (var item in select_sale)
                                {
                                    worksheet.Cell(row, 1).Value = item.date;
                                    worksheet.Cell(row, 2).Value = item.reference_no;
                                    worksheet.Cell(row, 3).Value = item.customer_.name;
                                    worksheet.Cell(row, 4).Value = item.sale_status;
                                    worksheet.Cell(row, 5).Value = String.Format("{0:C}", item.grand_total);
                                    worksheet.Cell(row, 6).Value = item.payment_status;
                                    worksheet.Cell(row, 7).Value = item.payment_method;
                                    
                                    row++;
                                }
                            }
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var contect = stream.ToArray();
                            fileName = "Sale.xlsx";
                            status = "export_to_excel";
                            excel_file = Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length);
                        }
                    }
                }
                if (key_action == "delete")
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        var obj = await _context.sma_sales.FindAsync(id[i]);
                        if (obj != null)
                        {
                            _context.Remove(obj);
                            await _context.SaveChangesAsync();
                            status = "1";
                            TempData["AlertMessageSuccess"] = "Sale deleted successful.";
                        }
                    }
                }
                if (key_action == "export_to_pdf")
                {
                    
                }

                return Json(
                    new {
                        status = status, 
                        stream_file = excel_file, 
                        fileName = fileName, 
                        action = key_action 
                    }
                );
            }
            else
                return Json(new { status = status });



            //using (var workbook = new XLWorkbook())
            //{
            //    var worksheet = workbook.Worksheets.Add("Student");

            //    // header
            //    worksheet.Cell(1, 1).Value = "Date";
            //    worksheet.Cell(1, 2).Value = "Reference No";
            //    worksheet.Cell(1, 3).Value = "Customer";
            //    worksheet.Cell(1, 4).Value = "Sale Status";
            //    worksheet.Cell(1, 5).Value = "Grand Total";
            //    worksheet.Cell(1, 6).Value = "Payment Status";
            //    worksheet.Cell(1, 7).Value = "Paid By";

            //    // body
            //    for (int i = 0; i < id.Length; i++)
            //    {
            //        var select_sale = _context.sma_sales.Where(a => a.id == id[i]);
            //        if (select_sale != null)
            //        {
            //            foreach (var item in select_sale)
            //            {
            //                worksheet.Cell(row, 1).Value = item.date;
            //                worksheet.Cell(row, 2).Value = item.reference_no;
            //                worksheet.Cell(row, 3).Value = item.customer_id;
            //                worksheet.Cell(row, 4).Value = item.sale_status;
            //                worksheet.Cell(row, 5).Value = item.grand_total;
            //                worksheet.Cell(row, 6).Value = item.payment_status;
            //                worksheet.Cell(row, 7).Value = item.payment_method;
            //                row++;
            //            }
            //        }
            //    }

            //    using (var stream = new MemoryStream())
            //    {
            //        workbook.SaveAs(stream);
            //        var contect = stream.ToArray();
            //        var fileName = "Sale.xlsx";

            //        return Json(new { status = "export_to_excel", result = contect, fileName = fileName });
            //    }
            //}

        }
    }
}
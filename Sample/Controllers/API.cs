using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;
using Sample.Data;

namespace Dashboard.Controllers
{
    [ApiController]
    [Route("api")]
    public class API : Controller
    {
        public readonly SampleContext _context;
        private readonly HttpClient _httpClient;
        private const string RequestUri = "http://localhost:5080/api/";
        public API(SampleContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }



        /*==========================
        =                          =
        =         API Sale         =
        =                          =
        ===========================*/

        [HttpGet]
        [Route("sale/view")]
        public IEnumerable<Sale> SaleGet()
        {
            return _context.sma_sales.ToList();
        }

        [HttpPost]
        [Route("sale/create")]
        public IActionResult SalePost(Sale s)
        {
            Random ran = new Random();
            int reference_no = ran.Next();
            double g = (s.total + s.shipping) - s.discount;
            _context.sma_sales.Add(new Sale()
            {
                date = DateTime.Now,
                reference_no = "SL-" + reference_no,
                total = s.total,
                shipping = s.shipping,
                discount = s.discount,
                grand_total = g,
                sale_status = s.sale_status,
                payment_method = s.payment_method,
                payment_status = s.payment_status,
                customer_id = s.customer_id,
                biller_id = s.biller_id
            });
            _context.SaveChanges();
            return Ok(new { status = "Data Inserted Successfull." });
        }

        [HttpDelete]
        [Route("sale/delete/{id}")]
        public IActionResult SaleDelete(int id)
        {
            Sale s = _context.sma_sales.Find(id);
            if (s != null && id > 0)
            {
                _context.sma_sales.Remove(s);
                _context.SaveChanges();
                return Ok(new { status = "Data Deleted Successfull." });
            }
            else
                return NotFound();
        }

        [HttpPut]
        [Route("sale/update/{id}")]
        public IActionResult SaleUpdate(int id, Sale s)
        {
            Sale a = _context.sma_sales.Where(x => x.id == id).FirstOrDefault<Sale>();
            if (a != null)
            {
                double g = (s.total + s.shipping) - s.discount;
                a.payment_status = s.payment_status;
                a.payment_method = s.payment_method;
                a.total = s.total;
                a.shipping = s.shipping;
                a.discount = s.discount;
                a.grand_total = g;
                _context.SaveChanges();
            }
            else
                return NotFound();

            return Ok(new { status = "Data Updated Successful." });
        }





        /*==========================
        =                          =
        =       API Student        =
        =                          =
        ===========================*/

        [HttpGet]
        [Route("student/view")]
        public IActionResult studentview()
        {
            HttpResponseMessage response = _httpClient.GetAsync(RequestUri + "student/view").Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return Ok(result);
        }

        [HttpPost]
        [Route("student/create")]
        public async Task<string> studentcreate()
        {
            var response = await _httpClient.GetAsync(RequestUri + "student/view");
            var contents = await response.Content.ReadAsStringAsync();
            string re;

            dynamic json = JsonConvert.DeserializeObject(contents);
            foreach (var item in json)
            {
                _context.sma_students.Add(new Student()
                {
                   Name = item.name,
                   Age = item.age,
                   Email = item.email,
                   DepartmentId = item.departmentId
                });
            }
            _context.SaveChanges();
            re = "Data Inserted.";


            return re.ToString();
        }
        [HttpDelete("student/delete/{id}")]
        //public async Task<string> deleteStudent(int id)
        public IActionResult deleteStudent(int id)
        {
            //var response = await _httpClient.DeleteAsync(RequestUri + "student/delete/" + id);
            //var contents = await response.Content.ReadAsStringAsync();

            //return contents;

            var s = _context.sma_students.Find(id);
            if (s != null)
            {
                _context.Remove(s);
                _context.SaveChanges();
                return Json(new { status = "Student Deleted Successfull." });
            }
            else
                return NotFound();
        }



        /*==========================
        =                          =
        =       API Product        =
        =                          =
        ===========================*/

        [HttpGet("product/views")]
        public IEnumerable<Product> ViewProduct()
        {
           return _context.sma_products.ToList();
        }
        [HttpPost("product/create")]
        public IActionResult CreateProduct(Product p)
        {
            _context.sma_products.Add(new Product
            {
                barcode = p.barcode,
                name = p.name,
                cost = p.cost,
                price = p.price,
                category_id = p.category_id
            });
            _context.SaveChanges();

            return Json(new {status = "Product Inserted Successfull."});
        }

        [HttpDelete("product/delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product p = _context.sma_products.Find(id);
            if (p != null)
            {
                _context.Remove(p);
                _context.SaveChanges();
                return Json(new { status = "Product Deleted Successfull." });
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("product/update/{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            var p = _context.sma_products.Where(x => x.id == id).FirstOrDefault();
            if (p != null)
            {
                p.barcode = product.barcode;
                p.name = product.name;
                p.price = product.price;
                p.category_id = product.category_id;
                p.cost = product.cost;
                _context.SaveChanges();

                return Json(new {status = "Product Updated Successfull."});
            }
            else
            { 
                return NotFound();
            }
        }


        [HttpPost("login")]
        public IActionResult login(User user)
        {
            var getUser = _context.sma_users
                .Where(x => x.username == user.username && x.password == user.password)
                .FirstOrDefault();

            if (getUser != null)
                return Json(new { status = "Login successfull." });
            else
                return Json(new { status = "Invalid username or password." });
        }
    }
}

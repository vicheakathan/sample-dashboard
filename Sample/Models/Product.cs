using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Product
    {
        public int id { get; set; }
        public string barcode { get; set; }
        public string name { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public string? image { get; set; }
        public int? category_id { get; set; }
        public Category? category_ { get; set; }
    }
}

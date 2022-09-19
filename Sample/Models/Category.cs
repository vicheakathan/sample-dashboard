namespace Dashboard.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Expense>? sma_expenses { get; set; }
        public List<Product>? sma_products { get; set; }
    }
}

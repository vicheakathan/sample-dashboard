namespace Dashboard.Models
{
    public class Expense
    {
        public int id { get; set; }
        public double amount { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public string description { get; set; }
        public int? category_id { get; set; }
        public Category? category_ { get; set; }
    }
}

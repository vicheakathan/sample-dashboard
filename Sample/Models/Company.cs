namespace Dashboard.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string company_name { get; set; }
        public int phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string? logo { get; set; }
        public List<Sale>? sma_sales { get; set; }
    }
}

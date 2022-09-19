namespace Dashboard.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string? activation_code { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int phone { get; set; }
        public string? gender { get; set; }
        public string? company { get; set; }
        public int? active { get; set; }
        public string? logo { get; set; }
        public List<Sale>? sma_sales { get; set; }
    }
}

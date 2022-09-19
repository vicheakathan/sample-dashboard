namespace Dashboard.Models
{
    public class Sale
    {
        public int id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public string reference_no { get; set; }
        public double total { get; set; }
        public double discount { get; set; }
        public double shipping { get; set; }
        public double grand_total { get; set; }
        public string sale_status { get; set; }
        public string payment_status { get; set; }
        public string payment_method { get; set; }
        public int? customer_id { get; set; }
        public int? biller_id { get; set; }
        public Company? customer_ { get; set; }
        public User? biller_ { get; set; }
    }
}

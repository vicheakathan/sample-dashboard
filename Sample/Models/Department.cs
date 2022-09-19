namespace Dashboard.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<Student>? Student { get; set; }
    }
}

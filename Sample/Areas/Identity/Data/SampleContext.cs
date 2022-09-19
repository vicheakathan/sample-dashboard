using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sample.Areas.Identity.Data;

namespace Sample.Data;

public class SampleContext : IdentityDbContext<SampleUser>
{
    internal readonly object tbl;

    public SampleContext(DbContextOptions<SampleContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Category> sma_categories { get; set; }
    public DbSet<Expense> sma_expenses { get; set; }
    public DbSet<Product> sma_products { get; set; }
    public DbSet<Sale> sma_sales { get; set; }
    public DbSet<Company> sma_companies { get; set; }
    public DbSet<User> sma_users { get; set; }
    public DbSet<Student> sma_students { get; set; }
    public DbSet<Department> sma_departments { get; set; }
}

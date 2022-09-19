using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sample.Data;
using Sample.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SampleContextConnection") ?? throw new InvalidOperationException("Connection string 'SampleContextConnection' not found.");

builder.Services.AddDbContext<SampleContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<SampleUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<SampleContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound/{0}");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});


app.Run();

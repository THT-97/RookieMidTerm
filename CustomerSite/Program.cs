using Ecommerce.CustomerSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//Add connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Add DbContext for indentity service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//Identity service
builder.Services.AddDefaultIdentity<IdentityUser>(
    //Identity service options
    options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
    }
).AddEntityFrameworkStores<ApplicationDbContext>();
//Add HttpClient
builder.Services.AddHttpClient("client",
    options => { options.BaseAddress = new Uri(builder.Configuration["apiUrl"]); });
//Add Session with idle timeout 1hr
builder.Services.AddSession(options=>options.IdleTimeout = TimeSpan.FromHours(1));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

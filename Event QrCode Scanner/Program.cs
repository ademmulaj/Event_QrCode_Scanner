using Event_QrCode_Scanner.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Konfigurimi i kontekstit
builder.Services.AddDbContext<Konteksti>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Lidhja")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Make sure to add this
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Specify the IP address and port for your application to listen on
var ipAddress = "192.168.1.6";
var port = 2506; // Use the desired port number

app.Run($"https://{ipAddress}:{port}");
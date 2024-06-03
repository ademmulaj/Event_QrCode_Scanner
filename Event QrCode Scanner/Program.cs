using Event_QrCode_Scanner.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// specifikimi i ip adreses aktuale te HOST-IT
var ipAddress = "192.168.1.29";
// vendos portin e deshirurar
var port = 8080; 

app.Run($"https://{ipAddress}:{port}");
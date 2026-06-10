using Microsoft.EntityFrameworkCore;
using PdfSharp.Fonts;
using WebShop;
using WebShop.Data;

var builder = WebApplication.CreateBuilder(args);

GlobalFontSettings.FontResolver = PdfFontResolver.Instance;

// Dodavanje MVC servisa
builder.Services.AddControllersWithViews();

// AKTIVACIJA SESIJE: Omogućuje aplikaciji privremeno pamćenje podataka (košarice)
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Košarica se prazni nakon 30 min neaktivnosti
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Dohvaćanje veze za XAMPP bazu podataka
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BazaDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();


// Konfiguracija HTTP zahtjeva
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// VAŽNO: Pokretanje sesije (mora biti prije UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
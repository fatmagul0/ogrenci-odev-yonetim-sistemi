using Microsoft.EntityFrameworkCore;
using OgrenciOdevYonetimSistemi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ VERİTABANI bağlanıyor (eksikti)
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Session konfigürasyonu
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
});
builder.Services
    .AddControllersWithViews()
    .AddSessionStateTempDataProvider(); // ✅ TempData'yı session ile kullanacağız

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // ✅ Session aktif
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

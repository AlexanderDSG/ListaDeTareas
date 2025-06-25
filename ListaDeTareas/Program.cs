using Microsoft.EntityFrameworkCore;
using ListaDeTareas.Data;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Usa rutas por defecto
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

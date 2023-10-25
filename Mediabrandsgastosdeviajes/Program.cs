using Mediabrandsgastosdeviajes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ViajesgastosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
// Configura la inyección de dependencia para IConverter
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Configurar cadena de conexión
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

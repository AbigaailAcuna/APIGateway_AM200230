using LibroAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ProductoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

// Registrar los servicios de DbContext
builder.Services.AddDbContext<LibroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ProductoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar controladores
builder.Services.AddControllers();

// Configuración del logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configurar middleware
app.UseRouting();
app.UseAuthorization();

// Middleware de Ocelot
await app.UseOcelot();

// Log de inicio
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Ocelot is running on port {Port}", builder.Configuration["GlobalConfiguration:BaseUrl"]?.Split(':').Last());

app.Run();

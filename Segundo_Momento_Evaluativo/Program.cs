using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Base de Datos con Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=PortalMicrocursos_UnivalleDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite", false);

if (useSqlite)
{
    var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=portal_microcursos.db";
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(sqliteConnection));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// 2. Servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Asegurar creación de base de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos.");
    }
}

// 3. Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cursos}/{action=Index}/{id?}");

app.Run();

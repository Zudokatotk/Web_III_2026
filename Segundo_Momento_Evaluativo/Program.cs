using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Data;
using Segundo_Momento_Evaluativo.Models;

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

// Asegurar creación de base de datos e inicializar datos de prueba
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.EnsureCreated();

        // Datos iniciales de prueba si la base de datos está vacía
        if (!context.Cursos.Any())
        {
            var curso1 = new Curso
            {
                Titulo = "Fundamentos de ASP.NET Core MVC",
                Descripcion = "Aprende la arquitectura Modelo-Vista-Controlador en .NET, enrutamiento, controladores y vistas Razor.",
                Nivel = "Principiante",
                DuracionMinutos = 120,
                EstaPublicado = true,
                FechaCreacion = DateTime.UtcNow,
                Lecciones = new List<Leccion>
                {
                    new Leccion { Titulo = "Arquitectura MVC y ciclo de vida de peticiones", Orden = 1, Contenido = "<h3>Patrón MVC</h3><p>El patrón separa la aplicación en Modelo, Vista y Controlador.</p>", FechaCreacion = DateTime.UtcNow },
                    new Leccion { Titulo = "Controladores y Enrutamiento", Orden = 2, Contenido = "<h3>Controladores</h3><p>Manejan las solicitudes HTTP entrantes.</p>", FechaCreacion = DateTime.UtcNow }
                }
            };

            var curso2 = new Curso
            {
                Titulo = "Entity Framework Core y Persistencia SQL",
                Descripcion = "Domina el enfoque Code-First, DbContext, relaciones entre entidades y consultas LINQ.",
                Nivel = "Intermedio",
                DuracionMinutos = 180,
                EstaPublicado = true,
                FechaCreacion = DateTime.UtcNow,
                Lecciones = new List<Leccion>
                {
                    new Leccion { Titulo = "Enfoque Code-First y DbContext", Orden = 1, Contenido = "<h3>Code-First</h3><p>Definición de tablas mediante clases C#.</p>", FechaCreacion = DateTime.UtcNow }
                }
            };

            context.Cursos.AddRange(curso1, curso2);
            context.SaveChanges();

            context.Inscripciones.Add(new Inscripcion
            {
                CursoId = curso1.Id,
                NombreEstudiante = "Juan Pérez",
                CorreoEstudiante = "juan.perez@univalle.edu",
                FechaInscripcion = DateTime.UtcNow
            });
            context.SaveChanges();
        }
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

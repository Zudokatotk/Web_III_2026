using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Models;

namespace Segundo_Momento_Evaluativo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Leccion> Lecciones { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<LeccionCompletada> LeccionesCompletadas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Curso -> Lecciones (Eliminación en cascada)
            modelBuilder.Entity<Leccion>()
                .HasOne(l => l.Curso)
                .WithMany(c => c.Lecciones)
                .HasForeignKey(l => l.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Curso -> Inscripciones
            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Curso)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restricción única: no duplicar inscripción por correo y curso
            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => new { i.CursoId, i.CorreoEstudiante })
                .IsUnique();

            // Relación Lección -> LeccionesCompletadas
            modelBuilder.Entity<LeccionCompletada>()
                .HasOne(lc => lc.Leccion)
                .WithMany(l => l.LeccionesCompletadas)
                .HasForeignKey(lc => lc.LeccionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restricción única: una lección completada por correo
            modelBuilder.Entity<LeccionCompletada>()
                .HasIndex(lc => new { lc.LeccionId, lc.CorreoEstudiante })
                .IsUnique();
        }
    }
}

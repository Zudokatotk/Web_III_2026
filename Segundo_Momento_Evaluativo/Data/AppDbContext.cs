using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Models;

namespace Segundo_Momento_Evaluativo.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
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

            modelBuilder.Entity<Leccion>()
                .HasOne(l => l.Curso)
                .WithMany(c => c.Lecciones)
                .HasForeignKey(l => l.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Curso)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey(i => i.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => new { i.CursoId, i.CorreoEstudiante })
                .IsUnique();

            modelBuilder.Entity<LeccionCompletada>()
                .HasOne(lc => lc.Leccion)
                .WithMany(l => l.LeccionesCompletadas)
                .HasForeignKey(lc => lc.LeccionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeccionCompletada>()
                .HasIndex(lc => new { lc.LeccionId, lc.CorreoEstudiante })
                .IsUnique();
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Segundo_Momento_Evaluativo.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede exceder 150 caracteres.")]
        [Display(Name = "Título del Curso")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nivel es obligatorio.")]
        [Display(Name = "Nivel")]
        public string Nivel { get; set; } = "Principiante"; // Principiante, Intermedio, Avanzado

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, 10000, ErrorMessage = "La duración debe ser mayor a 0 minutos.")]
        [Display(Name = "Duración estimada (minutos)")]
        public int DuracionMinutos { get; set; }

        [Display(Name = "¿Publicado?")]
        public bool EstaPublicado { get; set; } = false;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual ICollection<Leccion> Lecciones { get; set; } = new List<Leccion>();
        public virtual ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}

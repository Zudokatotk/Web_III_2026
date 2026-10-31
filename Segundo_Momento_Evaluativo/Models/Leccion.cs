using System.ComponentModel.DataAnnotations;

namespace Segundo_Momento_Evaluativo.Models
{
    public class Leccion
    {
        public int Id { get; set; }

        [Required]
        public int CursoId { get; set; }
        public virtual Curso? Curso { get; set; }

        [Required(ErrorMessage = "El título de la lección es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede exceder 150 caracteres.")]
        [Display(Name = "Título de la Lección")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contenido es obligatorio.")]
        [Display(Name = "Contenido (Texto o HTML sencillo)")]
        public string Contenido { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000, ErrorMessage = "El orden debe ser al menos 1.")]
        [Display(Name = "Orden")]
        public int Orden { get; set; } = 1;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedad de navegación
        public virtual ICollection<LeccionCompletada> LeccionesCompletadas { get; set; } = new List<LeccionCompletada>();
    }
}

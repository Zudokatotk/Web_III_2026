using System.ComponentModel.DataAnnotations;

namespace Segundo_Momento_Evaluativo.Models
{
    public class LeccionCompletada
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lección")]
        public int LeccionId { get; set; }
        public virtual Leccion? Leccion { get; set; }

        [Required(ErrorMessage = "El correo del estudiante es obligatorio.")]
        [EmailAddress]
        [Display(Name = "Correo del Estudiante")]
        public string CorreoEstudiante { get; set; } = string.Empty;

        [Display(Name = "Fecha de Completado")]
        public DateTime FechaCompletado { get; set; } = DateTime.UtcNow;
    }
}

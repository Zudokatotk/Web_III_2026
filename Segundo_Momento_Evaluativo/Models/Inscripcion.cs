using System.ComponentModel.DataAnnotations;

namespace Segundo_Momento_Evaluativo.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        public virtual Curso? Curso { get; set; }

        [Required(ErrorMessage = "El nombre del estudiante es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Display(Name = "Nombre del Estudiante")]
        public string NombreEstudiante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo del estudiante es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [Display(Name = "Correo Electrónico")]
        public string CorreoEstudiante { get; set; } = string.Empty;

        [Display(Name = "Fecha de Inscripción")]
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
    }
}

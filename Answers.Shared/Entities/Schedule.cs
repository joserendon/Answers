using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = null!;

        [Display(Description = "Detalle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(2000, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Description { get; set; } = null!;

        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public Questionnaire? Questionnaire { get; set; }

        public Guid QuestionnaireId { get; set; }

        public string? URLImage { get; set; }
    }
}

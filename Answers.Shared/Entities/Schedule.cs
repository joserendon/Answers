using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }

        [Display(Name = "Posible Respuesta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]

        public string Name { get; set; } = null!;

        [Display(Description = "Posible Respuesta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(2000, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]

        public string Description { get; set; } = null!;
        public DateTime? StardDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public Questionnaire? Questionnaire { get; set; }

        public Guid QuestionnaireId { get; set; }

        public string? URLImage { get; set;}

    }
}

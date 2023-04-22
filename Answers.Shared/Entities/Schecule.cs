using Prometheus;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Schecule
    {
        public Guid Id { get; set; }

        [Display(Name = "Posible Respuesta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]

        public string Name { get; set; } = null!;

        [Display(Description = "Posible Respuesta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(2000, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]

        public DateOnly? StardDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public BitArray? UserId { get; set; }

        public Questionnaire? QuestionnaireId { get; set; }

    }
}

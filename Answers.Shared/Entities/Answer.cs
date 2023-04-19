using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Posible Respuesta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = null!;

        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}

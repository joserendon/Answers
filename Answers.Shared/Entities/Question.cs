using Answers.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Name { get; set; } = null!;

        [Display(Name = "Tipo de pregunta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0, byte.MaxValue, ErrorMessage = "El campo {0} no es válido.")]
        public QuestionType Type { get; set; }

        public Guid AnswerId { get; set; }
        public Answer? Answer { get; set; }

        public ICollection<Questionnaire>? Questionnaires { get; set; }
        public int QuestionnaireNumber => Questionnaires == null ? 0 : Questionnaires.Count;
    }
}

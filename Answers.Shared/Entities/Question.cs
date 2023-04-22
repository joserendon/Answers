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
        [DataType(DataType.MultilineText)]
        public string Name { get; set; } = null!;

        [Display(Name = "Tipo de pregunta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, byte.MaxValue, ErrorMessage = "El campo {0} no es válido.")]
        public QuestionType Type { get; set; }

        public Guid QuestionnaireId { get; set; }
        public Questionnaire? Questionnaire { get; set; }

        public ICollection<Answer>? Answers { get; set; }
        public int QuestionnaireNumber => Answers == null ? 0 : Answers.Count;
    }
}

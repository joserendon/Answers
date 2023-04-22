using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Questionnaire
    {

        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string Title { get; set; } = null!;

        public ICollection<Question>? Questions { get; set; }
        public int QuestionsNumber => Questions == null ? 0 : Questions.Count;

        public ICollection<TemporalSchecule>? TemporalSchedules { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    internal class TemporalSchedule
    {
        public int Id { get; set; }

        public User? User { get; set; }

        public string? UserId { get; set; }

        public Questionnaire? Questionnaire { get; set; }

        public String QuestionnaireId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "1")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public decimal Value => Questionnaire == null ? 0 : 1;

    }
}

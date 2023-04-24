using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Poll
    {
        [Key]
        public Guid? Id { get; set; }

        public Guid? UserPollId { get; set; }

        [Display(Name = "programación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid? ScheduleId { get; set; }

        [Display(Name = "usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid? UserId { get; set; }

        [Display(Name = "cuestinoario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid? QuestionnaireId { get; set; }

        [Display(Name = "pregunta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid? QuestionId { get; set; }

        public DateTime? PollingDate { get; set; }

        public Reply? Reply { get; set; }
        public UserPoll? UserPoll { get; set; }
    }
}

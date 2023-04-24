using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class ReplyDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReplyId { get; set; }
        public Guid? AnswerId { get; set; }
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caractéres")]
        public string? OpenAnswer { get; set; }

        public Reply? Reply { get; set; }
    }
}

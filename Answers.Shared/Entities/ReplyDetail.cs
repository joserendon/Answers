using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class ReplyDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReplyId { get; set; }
        public Guid AnswerId { get; set; }

        public Reply? Reply { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class Reply
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PollId { get; set; }
        public DateTime? ReplyDate { get; set; }

        public ICollection<ReplyDetail>? ReplyDetails { get; set; }
        public Poll Poll { get; set; } = null!;
    }
}

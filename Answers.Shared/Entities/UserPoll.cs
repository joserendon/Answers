using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class UserPoll
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public bool IsCompleted { get; set; }

        public User User { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;
    }
}

using Answers.Shared.Entities;

namespace Answers.Shared.DTOs
{
    public class QuestionDTO : Question
    {
        public string? OpenAnswer { get; set; }
        public List<Guid?> ChoiceAnswers { get; set; } = new();
        public Guid? PollId { get; set; }
    }
}

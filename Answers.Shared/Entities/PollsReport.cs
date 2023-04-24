using Answers.Shared.Enums;

namespace Answers.Shared.Entities
{
    public class PollsReport
    {
        public long? Id { get; set; }

        public Guid? SCHEDULES_ID { get; set; }
        public string? SCHEDULES_NAME { get; set; }
        public string? SCHEDULES_DESCRIPTION { get; set; }
        public DateTime? STARTDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public bool ISACTIVE { get; set; }
        public Guid? USERS_ID { get; set; }
        public string? FIRSTNAME { get; set; }
        public string? LASTNAME { get; set; }
        public Guid? QUESTIONS_ID { get; set; }
        public string? QUESTIONS_NAME { get; set; }
        public QuestionType TYPE { get; set; }
        public string? ANSWERS_NAME { get; set; }
    }
}

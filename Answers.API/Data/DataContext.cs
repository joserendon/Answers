
using Answers.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, Guid>

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<UserPoll> UserPolls { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<ReplyDetail> ReplyDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(nameof(State.CountryId), nameof(State.Name)).IsUnique();
            modelBuilder.Entity<City>().HasIndex(nameof(City.StateId), nameof(City.Name)).IsUnique();

            modelBuilder.Entity<Questionnaire>().HasIndex(x => x.Title).IsUnique();
            modelBuilder.Entity<Question>().HasIndex(nameof(Question.QuestionnaireId), nameof(Question.Name)).IsUnique();
            modelBuilder.Entity<Answer>().HasIndex(nameof(Answer.QuestionId), nameof(Answer.Name)).IsUnique();
            modelBuilder.Entity<Schedule>().HasIndex(nameof(Schedule.QuestionnaireId), nameof(Schedule.Name), nameof(Schedule.StartDate)).IsUnique();

            modelBuilder.Entity<UserPoll>().HasIndex(nameof(UserPoll.ScheduleId), nameof(UserPoll.UserId)).IsUnique();
            modelBuilder.Entity<Poll>().HasIndex(nameof(Poll.ScheduleId), nameof(Poll.UserId), nameof(Poll.QuestionnaireId), nameof(Poll.QuestionId)).IsUnique();
            modelBuilder.Entity<ReplyDetail>().HasIndex(nameof(ReplyDetail.ReplyId), nameof(ReplyDetail.AnswerId)).IsUnique();

            modelBuilder.Entity<PollsReport>().ToSqlQuery("EXEC DBO.Polls_Report @ScheduleId");
        }
    }
}

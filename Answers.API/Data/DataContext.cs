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

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex("CountryId", "Name").IsUnique();
            modelBuilder.Entity<City>().HasIndex("StateId", "Name").IsUnique();

            modelBuilder.Entity<Answer>().HasIndex(x => x.Title).IsUnique();
            modelBuilder.Entity<Question>().HasIndex(nameof(Question.AnswerId), nameof(Question.Name)).IsUnique();
            modelBuilder.Entity<Questionnaire>().HasIndex(nameof(Questionnaire.QuestionId), nameof(Questionnaire.Name)).IsUnique();
        }
    }
}

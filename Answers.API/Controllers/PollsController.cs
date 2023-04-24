using Answers.API.Data;
using Answers.API.Helpers;
using Answers.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Controllers
{
    [ApiController]
    [Route("/api/polls")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PollsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        public PollsController(IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage, IMailHelper mailHelper, DataContext context)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _mailHelper = mailHelper;
            _context = context;
        }


        [HttpPost("CreateSurvey")]
        public async Task<ActionResult> CreateSurvey([FromBody] Schedule schedule)
        {
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var questionnaire = await _context.Questionnaires.Where(x => x.Id == schedule.QuestionnaireId).FirstOrDefaultAsync();
            if (questionnaire == null)
            {
                return NotFound();
            }

            var userPoll = await _context.UserPolls.Where(x => x.UserId == user.Id && x.ScheduleId == schedule.Id).FirstOrDefaultAsync();

            var userPollId = userPoll == null ? Guid.NewGuid() : userPoll.Id;

            var questionsPoll = await _context.Questions.Where(x => x.QuestionnaireId == questionnaire.Id)
                                                 .Select(x => new Poll
                                                 {
                                                     PollingDate = DateTime.UtcNow,
                                                     QuestionId = x.Id,
                                                     QuestionnaireId = x.QuestionnaireId,
                                                     ScheduleId = schedule.Id,
                                                     UserId = user.Id,
                                                     UserPollId = userPollId
                                                 })
                                                 .ToListAsync();

            if (questionsPoll is null || !questionsPoll.Any())
            {
                return NotFound("Encuesta sin preguntas asociadas");
            }

            var userPollModel = new UserPoll()
            {
                Id = userPollId,
                ScheduleId = schedule.Id,
                UserId = user.Id,
            };
            if (userPoll == null)
            {
                await _context.AddAsync(userPollModel);
                await _context.AddRangeAsync(questionsPoll);
                await _context.SaveChangesAsync();
            }

            return Ok(userPollModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(Guid pollId)
        {
            var polls = await _context.Polls.Where(x => x.UserPollId.Equals(pollId))
                                            .Include(x => x.UserPoll!)
                                            .ThenInclude(x => x.Schedule!)
                                            .ThenInclude(x => x.Questionnaire!)
                                            .ThenInclude(x => x.Questions!)
                                            .ThenInclude(x => x.Answers)
                                            .Include(x => x.Reply!)
                                            .ThenInclude(x => x.ReplyDetails)
                                            .FirstOrDefaultAsync();
            if (polls is null)
            {
                return NotFound();
            }



            //var polls = await _context.Questionnaires.Where(x => x.Id.Equals(poll.QuestionnaireId))
            //                                         .Include(x => x.Questions!)
            //                                         .ThenInclude(x => x.Answers)
            //                                         .ToListAsync();

            return Ok(polls);
        }
    }
}

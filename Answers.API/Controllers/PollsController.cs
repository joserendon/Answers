using Answers.API.Data;
using Answers.API.Helpers;
using Answers.Shared.DTOs;
using Answers.Shared.Entities;
using Answers.Shared.Enums;
using Answers.Shared.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace Answers.API.Controllers
{
    [ApiController]
    [Route("/api/polls")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PollsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        public PollsController(IUserHelper userHelper, IMailHelper mailHelper, DataContext context)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _context = context;
        }

        [HttpGet("GetUserPollAsync")]
        public async Task<ActionResult> GetUserPollAsync(Guid UserPollId)
        {
            var userPoll = await _context.UserPolls.Where(userPoll => userPoll.Id.Equals(UserPollId)).FirstOrDefaultAsync();

            if (userPoll is null)
            {
                return NotFound();
            }

            return Ok(userPoll);
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
                try
                {
                    await _context.AddAsync(userPollModel);
                    await _context.AddRangeAsync(questionsPoll);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok(userPollModel);
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(Guid UserPollId)
        {
            var polls = await _context.Polls.Where(poll => poll.UserPollId.Equals(UserPollId))
                                            .Include(userPoll => userPoll.UserPoll!)
                                                .ThenInclude(schedule => schedule.Schedule!)
                                                .ThenInclude(questionnaire => questionnaire.Questionnaire!)
                                                .ThenInclude(question => question.Questions!)
                                                .ThenInclude(answer => answer.Answers)
                                            .Include(reply => reply.Reply!)
                                                .ThenInclude(x => x.ReplyDetails)
                                            .ToListAsync();

            if (polls is null)
            {
                return NotFound();
            }

            return Ok(polls);
        }

        [HttpPost("SaveSurvey")]
        public async Task<ActionResult> SaveSurvey([FromQuery] Guid UserPollId, [FromBody] List<QuestionDTO> Answers)
        {
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }

            var replies = new List<Reply>();
            var replyDetails = new List<ReplyDetail>();

            foreach (var answer in Answers)
            {
                if (answer is null || answer.PollId is null)
                {
                    continue;
                }

                var replyId = Guid.NewGuid();
                replies.Add(new Reply()
                {
                    Id = replyId,
                    PollId = answer.PollId ?? Guid.Empty,
                    ReplyDate = DateTime.UtcNow
                });

                if (answer.Type is QuestionType.Open)
                {
                    replyDetails.Add(new ReplyDetail
                    {
                        ReplyId = replyId,
                        OpenAnswer = answer.OpenAnswer
                    });
                }

                if (answer.Type is QuestionType.Choice or QuestionType.Multiple)
                {
                    foreach (var answerChoiceId in answer.ChoiceAnswers)
                    {
                        if (answerChoiceId is null)
                        {
                            continue;
                        }

                        replyDetails.Add(new ReplyDetail
                        {
                            ReplyId = replyId,
                            AnswerId = answerChoiceId ?? Guid.Empty
                        });
                    }
                }
            }

            if (!replies.Any() && !replyDetails.Any())
            {
                return NotFound();
            }
            try
            {
                await _context.AddRangeAsync(replies);
                await _context.AddRangeAsync(replyDetails);
                await _context.SaveChangesAsync();
                await _context.UserPolls.Where(userPoll => userPoll.Id == UserPollId)
                                        .ExecuteUpdateAsync(sp => sp.SetProperty(userPoll => userPoll.IsCompleted, userPoll => true));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var response = _mailHelper.SendMail(user.FullName, user.Email!,
              $"Answers - Notificación de participación",
              $"<p>Graciar por participar en la encuesta sus respuestas han sido guardadas con éxito</p>");

            if (response.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpGet("LoadSurvey")]
        public async Task<ActionResult> LoadSurvey(Guid UserPollId)
        {
            var polls = await _context.Polls.Where(poll => poll.UserPollId.Equals(UserPollId))
                                            .Include(userPoll => userPoll.UserPoll!)
                                                .ThenInclude(schedule => schedule.Schedule!)
                                                .ThenInclude(questionnaire => questionnaire.Questionnaire!)
                                                .ThenInclude(question => question.Questions!)
                                            .ThenInclude(answer => answer.Answers)
                                            .Include(reply => reply.Reply!)
                                                .ThenInclude(x => x.ReplyDetails)
                                            .ToListAsync();

            if (polls is null)
            {
                return NotFound();
            }

            var pollsDto = polls.FirstOrDefault()?.UserPoll!.Schedule!.Questionnaire!.Questions!.Select(x => new QuestionDTO
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Answers = x.Answers,
                Questionnaire = x.Questionnaire,
                QuestionnaireId = x.QuestionnaireId,
                PollId = polls.Where(poll => poll.QuestionnaireId == x.QuestionnaireId && poll.QuestionId == x.Id)?.FirstOrDefault()?.Id!,
                OpenAnswer = polls.Where(poll => poll.QuestionnaireId == x.QuestionnaireId && poll.QuestionId == x.Id)?
                                  .FirstOrDefault()?.Reply?.ReplyDetails?
                                  .FirstOrDefault()?.OpenAnswer,
                ChoiceAnswers = polls.Where(poll => poll.QuestionnaireId == x.QuestionnaireId && poll.QuestionId == x.Id)?
                                     .FirstOrDefault()?.Reply?.ReplyDetails?
                                     .Select(x => x.AnswerId)?
                                     .ToList() ?? new(),
            }).ToList();

            return Ok(pollsDto);
        }

        [HttpGet("GetPollReportAsync")]
        public async Task<ActionResult> GetPollReportAsync(Guid ScheduleId)
        {
            var dataReport = await _context.Set<PollsReport>().FromSqlInterpolated($"EXEC DBO.Polls_Report @ScheduleId={ScheduleId}").ToListAsync();

            if (dataReport is null)
            {
                return NotFound();
            }

            var report = Export.ExportExcel(dataReport, out string fileName);

            return File(report, MediaTypeNames.Application.Octet, fileName);
        }
    }
}

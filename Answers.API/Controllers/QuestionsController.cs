using Answers.API.Data;
using Answers.API.Helpers;
using Answers.Shared.DTOs;
using Answers.Shared.Entities;
using Answers.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly DataContext _context;

        public QuestionsController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo/{questionnaireId:guid}")]
        public async Task<ActionResult> GetCombo(Guid questionnaireId)
        {
            return Ok(await _context.Questions
                                    .Where(x => x.QuestionnaireId == questionnaireId)
                                    .ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Questions
                                    .Include(x => x.Answers)
                                    .Where(x => x.Questionnaire!.Id == pagination.Id_Guid)
                                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable.OrderBy(x => x.Name)
                                     .Paginate(pagination)
                                     .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Questions.Where(x => x.Questionnaire!.Id == pagination.Id_Guid)
                                              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var question = await _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Question question)
        {
            _context.Add(question);
            await _context.SaveChangesAsync();

            await RemoveAnswersInTypeOpen(question);

            return Ok(question);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Question question)
        {
            _context.Update(question);
            await _context.SaveChangesAsync();

            await RemoveAnswersInTypeOpen(question);

            return Ok(question);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Remove(question);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task RemoveAnswersInTypeOpen(Question question)
        {
            if (question.Type is QuestionType.Open)
            {
                await _context.Answers.Where(answers => answers.QuestionId == question.Id).ExecuteDeleteAsync();
            }
        }

    }
}

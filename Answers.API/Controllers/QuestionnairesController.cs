using Answers.API.Data;
using Answers.API.Helpers;
using Answers.Shared.DTOs;
using Answers.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Answers.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/questionnaires")]
    public class QuestionnairesController : ControllerBase
    {
        private readonly DataContext _context;

        public QuestionnairesController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo/{questionId:Guid}")]
        public async Task<ActionResult> GetCombo(Guid questionId)
        {
            return Ok(await _context.Questionnaires
                .Where(x => x.QuestionId == questionId)
                .ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Questionnaires
                .Where(x => x.Question!.Id == pagination.Id_Guid)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }


        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Questionnaires
                .Where(x => x.Question!.Id == pagination.Id_Guid)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var questionnaire = await _context.Questionnaires.FirstOrDefaultAsync(x => x.Id == id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            return Ok(questionnaire);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Questionnaire questionnaire)
        {
            _context.Add(questionnaire);
            await _context.SaveChangesAsync();
            return Ok(questionnaire);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Questionnaire questionnaire)
        {
            _context.Update(questionnaire);
            await _context.SaveChangesAsync();
            return Ok(questionnaire);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var questionnaire = await _context.Questionnaires.FirstOrDefaultAsync(x => x.Id == id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            _context.Remove(questionnaire);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

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
    [Route("/api/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly DataContext _context;

        public AnswersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Answers.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Answers
                .Include(x => x.Questions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Title.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Title)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Answers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Title.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.Answers
                .Include(x => x.Questions!)
                .ThenInclude(x => x.Questionnaires)
                .ToListAsync());
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var answer = await _context.Answers
                .Include(x => x.Questions!)
                .ThenInclude(x => x.Questionnaires)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Answer answer)
        {
            _context.Add(answer);
            await _context.SaveChangesAsync();
            return Ok(answer);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Answer answer)
        {
            _context.Update(answer);
            await _context.SaveChangesAsync();
            return Ok(answer);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var answer = await _context.Answers.FirstOrDefaultAsync(x => x.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Remove(answer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

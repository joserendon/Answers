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

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Answers.Where(x => x.Question!.Id == pagination.Id_Guid)
                                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower()
                                     .Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync());
        }


        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Answers.Where(x => x.Question!.Id == pagination.Id_Guid)
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
            var answer = await _context.Answers.FirstOrDefaultAsync(x => x.Id == id);
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

        [HttpDelete("{id:guid}")]
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

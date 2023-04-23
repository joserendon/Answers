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
    [Route("/api/schedules")]
    public class SchedulesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;
        private readonly string _container;


        public SchedulesController(DataContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
            _container = "schedules";
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Schedules.ToListAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Schedules
                                    .Include(x => x.Questionnaire!)
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
        [AllowAnonymous]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Schedules.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetFullAsync()
        {
            return Ok(await _context.Schedules
                                    .Include(x => x.Questionnaire!)
                                    .ThenInclude(x => x.Questions!)
                                    .ThenInclude(x => x.Answers)
                                    .ToListAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var schedule = await _context.Schedules
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Schedule schedule)
        {
            if (!string.IsNullOrEmpty(schedule.URLImage))
            {
                var img = Convert.FromBase64String(schedule.URLImage);
                schedule.URLImage = await _fileStorage.SaveFileAsync(img, ".jpg", _container);
            }

            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Schedule schedule)
        {
            if (!string.IsNullOrEmpty(schedule.URLImage))
            {
                var img = Convert.FromBase64String(schedule.URLImage);
                schedule.URLImage = await _fileStorage.SaveFileAsync(img, ".jpg", _container);
            }

            _context.Update(schedule);
            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Remove(schedule);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

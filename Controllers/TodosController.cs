using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApiDotNet.Models;

namespace TodoApiDotNet.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TodosController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? done)
        {
            var query = _db.Todos.AsQueryable();
            if (done.HasValue) query = query.Where(t => t.IsDone == done.Value);
            return Ok(await query.OrderBy(t => t.CreatedAt).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.Todos.FindAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem input)
        {
            if (string.IsNullOrWhiteSpace(input.Title) || input.Title.Length > 200)
                return BadRequest("Title is required (max 200 chars).");

            input.Id = 0;
            input.CreatedAt = DateTime.UtcNow;
            input.IsDone = false;
            _db.Todos.Add(input);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItem input)
        {
            var item = await _db.Todos.FindAsync(id);
            if (item is null) return NotFound();

            if (string.IsNullOrWhiteSpace(input.Title) || input.Title.Length > 200)
                return BadRequest("Title is required (max 200 chars).");

            item.Title = input.Title;
            item.IsDone = input.IsDone;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Todos.FindAsync(id);
            if (item is null) return NotFound();
            _db.Todos.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
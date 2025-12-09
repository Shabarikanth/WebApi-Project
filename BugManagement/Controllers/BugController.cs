using Microsoft.AspNetCore.Mvc;
using BugManagement.Data;
using BugManagement.Models;

namespace BugManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BugController : ControllerBase
    {
        private readonly BugRepository _repo;
        private readonly ILogger<BugController> _logger;

        public BugController(BugRepository repo, ILogger<BugController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: api/bugs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // GET: api/bugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bug = await _repo.GetByIdAsync(id);
            if (bug == null)
                return NotFound(new { message = "Bug not found" });

            return Ok(bug);
        }

        // POST: api/bugs
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBugDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(new { message = "Title is required" });

            var id = await _repo.CreateAsync(dto);
            var created = await _repo.GetByIdAsync(id);

            return CreatedAtAction(nameof(Get), new { id }, created);
        }

        // PUT: api/bugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBugDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest(new { message = "Title is required" });

            var updated = await _repo.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { message = "Bug not found" });

            return NoContent();
        }

        // DELETE: api/bugs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Bug not found" });

            return NoContent();
        }
    }
}

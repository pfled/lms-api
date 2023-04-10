using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleLMS.API.Models;
using SimpleLMS.API.Data;


namespace SimpleLMS.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly SimpleLMSContext _context;

        public AssignmentsController(SimpleLMSContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
        {
            if (_context.Assignments == null)
            {
                return NotFound();
            }
            return await _context.Assignments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        [HttpPost]
        public async Task<ActionResult<Assignment>> CreateAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssignment", new { id = assignment.ID }, assignment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, Assignment updatedAssignment)
        {
            if (id != updatedAssignment.ID)
            {
                return BadRequest();
            }

            _context.Entry(updatedAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.ID == id);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleLMS.API.Models;
using SimpleLMS.API.Data;

namespace SimpleLMS.API.Controllers {
    [ApiController]
    [Route("api/courses/{courseId}/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly SimpleLMSContext _context;

        public ModulesController(SimpleLMSContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Module>>> GetModules()
        {
            if (_context.Modules == null)
            {
                return NotFound();
            }
            return await _context.Modules.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int id)
        {
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.ID == id);

            if (module == null)
            {
                return NotFound();
            }

            return module;
        }

        [HttpPost]
        public async Task<ActionResult<Module>> CreateModule(Module module)
        {
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModule", new { id = module.ID }, module);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, Module updatedModule)
        {
            if (id != updatedModule.ID)
            {
                return BadRequest();
            }

            _context.Entry(updatedModule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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
        public async Task<IActionResult> DeleteModule(int id)
        {
            var module = await _context.Modules.FindAsync(id);

            if (module == null)
            {
                return NotFound();
            }

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.ID == id);
        }
    }
}

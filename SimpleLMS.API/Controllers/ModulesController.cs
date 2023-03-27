using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleLMS.API.Models;
using SimpleLMS.API.Data;

namespace SimpleLMS.API.Controllers {
    [ApiController]
    [Route("api/courses/{courseId}/[controller]")]
    public class ModulesController : ControllerBase
    {
        private static List<Module> _modules = DataStore.Modules;
        private static List<Course> _courses = DataStore.Courses;

        [HttpGet]
        public ActionResult<IEnumerable<Module>> GetModules(int courseId)
        {
            return _modules.FindAll(m => m.Course.ID == courseId);
        }

        [HttpGet("{id}")]
        public ActionResult<Module> GetModule(int courseId, int id)
        {
            var module = _modules.Find(m => m.Course.ID == courseId && m.ID == id);

            if (module == null)
            {
                return NotFound();
            }

            return module;
        }

        [HttpPost]
        public ActionResult<Module> CreateModule(int courseId, Module module)
        {
            var course = _courses.Find(c => c.ID == courseId);

            if (course == null)
            {
                return NotFound();
            }

            module.Course = course;
            _modules.Add(module);

            return CreatedAtAction(nameof(GetModule), new { courseId, id = module.ID }, module);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateModule(int courseId, int id, Module updatedModule)
        {
            var module = _modules.Find(m => m.Course.ID == courseId && m.ID == id);

            if (module == null)
            {
                return NotFound();
            }

            module.Name = updatedModule.Name;
            module.Assignments = updatedModule.Assignments;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteModule(int courseId, int id)
        {
            var module = _modules.Find(m => m.Course.ID == courseId && m.ID == id);

            if (module == null)
            {
                return NotFound();
            }

            _modules.Remove(module);

            return NoContent();
        }

    }
}

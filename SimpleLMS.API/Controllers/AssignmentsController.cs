using Microsoft.AspNetCore.Mvc;
using SimpleLMS.API.Models;
using SimpleLMS.API.Data;


namespace SimpleLMS.API.Controllers {
    [ApiController]
    [Route("api/courses/{courseId}/modules/{moduleId}/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private static List<Assignment> _assignments = DataStore.Assignments;
        private static List<Module> _modules = DataStore.Modules;

        [HttpGet]
        public ActionResult<IEnumerable<Assignment>> GetAssignments(int courseId, int moduleId)
        {
            return _assignments.FindAll(a => a.Module.ID == moduleId);
        }

        [HttpGet("{id}")]
        public ActionResult<Assignment> GetAssignment(int courseId, int moduleId, int id)
        {
            var assignment = _assignments.Find(a => a.Module.ID == moduleId && a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        [HttpPost]
        public ActionResult<Assignment> CreateAssignment(int courseId, int moduleId, Assignment assignment)
        {
            var module = _modules.Find(m => m.Course.ID == courseId && m.ID == moduleId);

            if (module == null)
            {
                return NotFound();
            }

            assignment.Module = module;
            _assignments.Add(assignment);

            return CreatedAtAction(nameof(GetAssignment), new { courseId, moduleId, id = assignment.ID }, assignment);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAssignment(int courseId, int moduleId, int id, Assignment updatedAssignment)
        {
            var assignment = _assignments.Find(a => a.Module.ID == moduleId && a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            assignment.Name = updatedAssignment.Name;
            assignment.Grade = updatedAssignment.Grade;
            assignment.DueDate = updatedAssignment.DueDate;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAssignment(int courseId, int moduleId, int id)
        {
            var assignment = _assignments.Find(a => a.Module.ID == moduleId && a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            _assignments.Remove(assignment);

            return NoContent();
        }
    }
}
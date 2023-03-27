using Microsoft.AspNetCore.Mvc;
using SimpleLMS.API.Data;
using SimpleLMS.API.Models;


namespace SimpleLMS.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private static List<Course> _courses = DataStore.Courses;

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            return _courses;
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetCourse(int id)
        {
            var course = _courses.Find(c => c.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPost]
        public ActionResult<Course> CreateCourse(Course course)
        {
            if (course == null)
            {
                return BadRequest();
            }

            _courses.Add(course);

            return CreatedAtAction(nameof(GetCourse), new { id = course.ID }, course);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, Course updatedCourse)
        {
            var course = _courses.Find(c => c.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            course.Name = updatedCourse.Name;
            course.Modules = updatedCourse.Modules;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courses.Find(c => c.ID == id);

            if (course == null)
            {
                return NotFound();
            }

            _courses.Remove(course);

            return NoContent();
        }

    }
}

using Xunit;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace SimpleLMS.Tests
{
    public class CoursesControllerTests
    {
        private CoursesController _controller = new CoursesController();
        private static List<Course> _coursesData = new List<Course>();

        [Fact]
        public void GetCourses_ShouldReturnCourses()
        {
            var result = _controller.GetCourses();

            Assert.IsType<ActionResult<IEnumerable<Course>>>(result);
        }

        [Fact]
        public void CreateCourse_ShouldAddNewCourse()
        {
            Course course = new Course { ID = 1, Name = "Test Course" };

            var result = _controller.CreateCourse(course);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public void UpdateCourse_ShouldModifyCourse()
        {
            Course course = new Course { ID = 1, Name = "Test Course" };
            _controller.CreateCourse(course);
            Course updatedCourse = new Course { Name = "Updated Course" };

            var result = _controller.UpdateCourse(1, updatedCourse);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCourse_ShouldRemoveCourse()
        {
            Course course = new Course { ID = 1, Name = "Test Course" };
            _controller.CreateCourse(course);

            var result = _controller.DeleteCourse(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}

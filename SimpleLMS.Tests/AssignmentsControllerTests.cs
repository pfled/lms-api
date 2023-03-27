using Xunit;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLMS.Tests
{
    public class AssignmentsControllerControllerTests
    {
        private AssignmentsController _assignmentsController;
        private CoursesController _coursesController;
        private ModulesController _modulesController;

        public AssignmentsControllerControllerTests() {
            _assignmentsController = new AssignmentsController();
            _coursesController = new CoursesController();
            _modulesController = new ModulesController();

            Course course = new Course { ID = 1, Name = "Test Course" };
            _coursesController.CreateCourse(course);

            Module module = new Module { ID = 1, Name = "Test Module", Course = course };
            _modulesController.CreateModule(course.ID, module);
        }

        [Fact]
        public void GetAssignments_ShouldReturnAssignments()
        {
            var result = _assignmentsController.GetAssignments(1, 1);

            Assert.IsType<ActionResult<IEnumerable<Assignment>>>(result);
        }

        [Fact]
        public void CreateAssignment_ShouldAddNewAssignment()
        {
            Assignment assignment = new Assignment { ID = 1, Name = "Test Assignment" };

            var result = _assignmentsController.CreateAssignment(1, 1, assignment);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public void UpdateAssignment_ShouldModifyAssignment()
        {
            Assignment assignment = new Assignment { ID = 1, Name = "Test Assignment" };
            _assignmentsController.CreateAssignment(1, 1, assignment);

            Assignment updatedAssignment = new Assignment { Name = "Updated Assignment" };

            var result = _assignmentsController.UpdateAssignment(1, 1, 1, updatedAssignment);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteAssignment_ShouldRemoveAssignment()
        {
            Assignment assignment = new Assignment { ID = 1, Name = "Test Assignment" };
            _assignmentsController.CreateAssignment(1, 1, assignment);

            var result = _assignmentsController.DeleteAssignment(1, 1, 1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}

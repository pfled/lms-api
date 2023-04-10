using System.Net.Http;
using Xunit;
using SimpleLMS.API.Models;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleLMS.API.Tests {
    public class ApiControllerTests
    {
        // *** REQUIRED EXTRA TESTS *** //

        // Create a course, and read the same course as a single entity, assert.
        [Fact]
        private async Task PostCourse_AndGetCourseTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostCourse_AndGetCourseTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                
                await coursesController.CreateCourse(newCourse);

                var result = await coursesController.GetCourse(newCourse.ID);

                var fetchedCourse = result.Value;
                
                Assert.Equal(newCourse.ID, fetchedCourse.ID);
                Assert.Equal(newCourse.Name, fetchedCourse.Name);
            }
        }

        // Create a course with two modules, and read the course with its related modules, assert.
        [Fact]
        private async Task PostCourse_TwoModulesTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostCourse_TwoModulesTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var modules = new List<Module>();

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = modules };

                modules.Add(new Module {
                    ID = 1, Name = "Week 1", Course = newCourse, Assignments = new List<Assignment>()
                });

                modules.Add(new Module {
                    ID = 2, Name = "Week 2", Course = newCourse, Assignments = new List<Assignment>()
                });
                
                await coursesController.CreateCourse(newCourse);

                var result = await coursesController.GetCourse(newCourse.ID);

                var fetchedCourse = result.Value;
                
                Assert.Equal(modules[0].Name, fetchedCourse.Modules[0].Name);
                Assert.Equal(modules[1].Name, fetchedCourse.Modules[1].Name);
            }
        }

        // Create three courses, and read all three courses, assert.
        [Fact]
        private async Task PostThreeCoursesTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostThreeCoursesTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse1 = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                var newCourse2 = new Course { ID = 2, Name = "CS 498", Modules = new List<Module>() };
                var newCourse3 = new Course { ID = 3, Name = "CS 380", Modules = new List<Module>() };
                
                await coursesController.CreateCourse(newCourse1);
                await coursesController.CreateCourse(newCourse2);
                await coursesController.CreateCourse(newCourse3);

                var result = await coursesController.GetCourses();

                var updatedCourseList = result.Value.ToList();

                Assert.Equal("CS 420", updatedCourseList[0].Name);
                Assert.Equal("CS 498", updatedCourseList[1].Name);
                Assert.Equal("CS 380", updatedCourseList[2].Name);
            }
        }

        // Create three assignments without any connection to a module, delete one, read all assignments, assert.
        [Fact]
        private async Task PostThreeAssignments_AndManipulateTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostThreeAssignments_AndManipulateTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment1 = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };
                var newAssignment2 = new Assignment{ ID = 2, Name = "Domain Model", DueDate = new DateTime(), Grade = "F", Module = null };
                var newAssignment3 = new Assignment{ ID = 3, Name = "Model Binding", DueDate = new DateTime(), Grade = "C-", Module = null };

                await assignmentsController.CreateAssignment(newAssignment1);
                await assignmentsController.CreateAssignment(newAssignment2);
                await assignmentsController.CreateAssignment(newAssignment3);

                await assignmentsController.DeleteAssignment(newAssignment2.ID);
                
                var result = await assignmentsController.GetAssignments();
                
                var updatedAssignmentsList = result.Value.ToList();

                Assert.Equal("Simple LMS API", updatedAssignmentsList[0].Name);
                Assert.Equal("Model Binding", updatedAssignmentsList[1].Name);
            }
        }

        // Create a course, add two modules, update a module's name, read the updated module within the course, assert.
        [Fact]
        private async Task PostNewCourse_WithModules_UpdateModuleNameTest() 
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostNewCourse_WithModules_UpdateModuleNameTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);
                var modulesController = new ModulesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                await coursesController.CreateCourse(newCourse);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = newCourse, Assignments = new List<Assignment>() };
                await modulesController.CreateModule(newModule);

                // Make sure module was added with original name
                var fetchModuleResult = await modulesController.GetModule(newModule.ID);
                var fetchedModule = fetchModuleResult.Value;
                Assert.Equal(newModule.Name, fetchedModule.Name);

                var updatedModuleName = "Week 1: The Sequel";

                fetchedModule.Name = updatedModuleName;
                await modulesController.UpdateModule(1, fetchedModule);

                var fetchCourseResult = await coursesController.GetCourse(newCourse.ID);

                var fetchedCourse = fetchCourseResult.Value;
                
                Assert.Equal(updatedModuleName, fetchedCourse.Modules[0].Name);
            }
        }

        // Create a course with a module and an assignment, update the assignment's due date, read the updated assignment within the module, assert.
        [Fact]
        private async Task PostCourseModuleAssignment_ChangeAssignmentDueDateTest() {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostCourseModuleAssignment_ChangeAssignmentDueDateTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);
                var modulesController = new ModulesController(context);
                var assignmentsController = new AssignmentsController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                await coursesController.CreateCourse(newCourse);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = newCourse, Assignments = new List<Assignment>() };
                await modulesController.CreateModule(newModule);

                var newAssignment = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = newModule };
                await assignmentsController.CreateAssignment(newAssignment);

                // Make sure assignment was added with original name
                var fetchAssignmentResult = await assignmentsController.GetAssignment(newAssignment.ID);
                var fetchedAssignment = fetchAssignmentResult.Value;
                Assert.Equal(newAssignment.Name, fetchedAssignment.Name);

                var updatedAssignmentName = "Simple LMS API: The Sequel";

                fetchedAssignment.Name = updatedAssignmentName;
                await assignmentsController.UpdateAssignment(1, fetchedAssignment);

                var fetchCourseResult = await coursesController.GetCourse(newCourse.ID);

                var fetchedCourse = fetchCourseResult.Value;
                
                Assert.Equal(updatedAssignmentName, fetchedCourse.Modules[0].Assignments[0].Name);
            }
        }

        // Create a course with a module and multiple assignments, delete one assignment, read the module with its remaining assignments, assert.
        [Fact]
        private async Task PostMultipleAssignments_DeleteAssignmentTest() {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostMultipleAssignments_DeleteAssignmentTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);
                var modulesController = new ModulesController(context);
                var assignmentsController = new AssignmentsController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                await coursesController.CreateCourse(newCourse);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = newCourse, Assignments = new List<Assignment>() };
                await modulesController.CreateModule(newModule);

                var newAssignment1 = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = newModule };
                var newAssignment2 = new Assignment{ ID = 2, Name = "Domain Model", DueDate = new DateTime(), Grade = "F", Module = newModule };
                var newAssignment3 = new Assignment{ ID = 3, Name = "Model Binding", DueDate = new DateTime(), Grade = "C-", Module = newModule };
                await assignmentsController.CreateAssignment(newAssignment1);
                await assignmentsController.CreateAssignment(newAssignment2);
                await assignmentsController.CreateAssignment(newAssignment3);

                await assignmentsController.DeleteAssignment(newAssignment1.ID);

                var fetchCourseResult = await coursesController.GetCourse(newCourse.ID);
                var fetchedCourse = fetchCourseResult.Value;
                
                Assert.Equal(2, fetchedCourse.Modules[0].Assignments.Count);
            }
        }
    }
}

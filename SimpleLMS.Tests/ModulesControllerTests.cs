using Xunit;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLMS.Tests
{
    public class ModulesControllerControllerTests
    {
        private CoursesController _coursesController;
        private ModulesController _modulesController;

        public ModulesControllerControllerTests() {
            _coursesController = new CoursesController();
            _modulesController = new ModulesController();

            Course course = new Course { ID = 1, Name = "Test Course" };
            _coursesController.CreateCourse(course);
        }

        [Fact]
        public void GetModules_ShouldReturnModules()
        {
            var result = _modulesController.GetModules(1);

            Assert.IsType<ActionResult<IEnumerable<Module>>>(result);
        }

        [Fact]
        public void CreateModule_ShouldAddNewModule()
        {
            Module module = new Module { ID = 1, Name = "Test Module" };

            var result = _modulesController.CreateModule(1, module);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public void UpdateModule_ShouldModifyModule()
        {
            Module module = new Module { ID = 1, Name = "Test Module" };
            _modulesController.CreateModule(1, module);
            Module updatedModule = new Module { Name = "Updated Module" };

            var result = _modulesController.UpdateModule(1, 1, updatedModule);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteModule_ShouldRemoveModule()
        {
            Module module = new Module { ID = 1, Name = "Test Module" };
            _modulesController.CreateModule(1, module);

            var result = _modulesController.DeleteModule(1, 1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
using System.Net.Http;
using Xunit;
using SimpleLMS.API.Models;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleLMS.API.Tests {
    public class ModulesControllerTests
    {
        [Fact]
        private async Task PostModuleTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostModuleTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var modulesController = new ModulesController(context);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = null, Assignments = new List<Assignment>() };
                
                await modulesController.CreateModule(newModule);

                var createdModule = await context.Modules.FindAsync(newModule.ID);
                
                Assert.Equal(newModule.ID, createdModule.ID);
                Assert.Equal(newModule.Name, createdModule.Name);
            }
        }

        [Fact]
        private async Task GetModuleTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetModuleTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var modulesController = new ModulesController(context);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = null, Assignments = new List<Assignment>() };
                
                context.Modules.Add(newModule);
                await context.SaveChangesAsync();

                var result = await modulesController.GetModule(newModule.ID);

                var fetchedModule = result.Value;
                
                Assert.Equal(newModule.Name,fetchedModule.Name);
            }
        }

        [Fact]
        private async Task GetModulesTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetModulesTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var modulesController = new ModulesController(context);

                var newModule1 = new Module { ID = 1, Name = "Week 1", Course = null, Assignments = new List<Assignment>() };
                var newModule2 = new Module { ID = 2, Name = "Week 2", Course = null, Assignments = new List<Assignment>() };
                
                context.Modules.Add(newModule1);
                context.Modules.Add(newModule2);
                await context.SaveChangesAsync();

                var result = await modulesController.GetModules();

                var fetchedModules = result.Value.ToList();
                
                Assert.Equal(newModule1.Name, fetchedModules[0].Name);
                Assert.Equal(newModule2.Name, fetchedModules[1].Name);
            }
        }

        [Fact]
        private async Task UpdateModuleTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "UpdateModuleTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var modulesController = new ModulesController(context);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = null, Assignments = new List<Assignment>() };
                
                context.Modules.Add(newModule);
                await context.SaveChangesAsync();

                // Make sure module was added with original name
                var createdModule = await context.Modules.FindAsync(newModule.ID);
                Assert.Equal(newModule.Name, createdModule.Name);

                var updatedModuleName = "Week 1: The Sequel";

                createdModule.Name = updatedModuleName;
                await modulesController.UpdateModule(1, createdModule);

                // Check for the new name
                var updatedModuleInDatabase = await context.Modules.FindAsync(createdModule.ID);
                Assert.Equal(updatedModuleName, updatedModuleInDatabase.Name);
            }
        }

        [Fact]
        private async Task DeleteModuleTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "DeleteModuleTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var modulesController = new ModulesController(context);

                var newModule = new Module { ID = 1, Name = "Week 1", Course = null, Assignments = new List<Assignment>() };
                
                context.Modules.Add(newModule);
                await context.SaveChangesAsync();

                // Make sure module was added to context
                var createdModule = await context.Modules.FindAsync(newModule.ID);
                Assert.Equal(newModule.Name, createdModule.Name);

                await modulesController.DeleteModule(newModule.ID);

                // Make sure it's gone now
                Assert.Null(await context.Modules.FindAsync(newModule.ID));
            }
        }
    }
}

using System.Net.Http;
using Xunit;
using SimpleLMS.API.Models;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleLMS.API.Tests {
    public class AssignmentsControllerTests
    {
        [Fact]
        private async Task PostAssignmentTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostAssignmentTest")
                .Options;

            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };

                await assignmentsController.CreateAssignment(newAssignment);

                var createdAssignment = await context.Assignments.FindAsync(newAssignment.ID);
                
                Assert.Equal(newAssignment.ID, createdAssignment.ID);
                Assert.Equal(newAssignment.Name, createdAssignment.Name);
            }
        }

        [Fact]
        private async Task GetAssignmentTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetAssignmentTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };
                
                context.Assignments.Add(newAssignment);
                await context.SaveChangesAsync();

                var result = await assignmentsController.GetAssignment(newAssignment.ID);

                var fetchedAssignment = result.Value;
                
                Assert.Equal(newAssignment.Name,fetchedAssignment.Name);
            }
        }

        [Fact]
        private async Task GetAssignmentsTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetAssignmentsTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment1 = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };
                var newAssignment2 = new Assignment{ ID = 2, Name = "Domain Model", DueDate = new DateTime(), Grade = "F", Module = null };
                
                context.Assignments.Add(newAssignment1);
                context.Assignments.Add(newAssignment2);
                await context.SaveChangesAsync();

                var result = await assignmentsController.GetAssignments();

                var fetchedAssignments = result.Value.ToList();
                
                Assert.Equal(newAssignment1.Name, fetchedAssignments[0].Name);
                Assert.Equal(newAssignment2.Name, fetchedAssignments[1].Name);
            }
        }

        [Fact]
        private async Task UpdateAssignmentTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAssignmentTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };
                
                context.Assignments.Add(newAssignment);
                await context.SaveChangesAsync();

                // Make sure assignment was added with original name
                var createdAssignment = await context.Assignments.FindAsync(newAssignment.ID);
                Assert.Equal(newAssignment.Name, createdAssignment.Name);

                var updatedAssignmentName = "Week 1: The Sequel";

                createdAssignment.Name = updatedAssignmentName;
                await assignmentsController.UpdateAssignment(1, createdAssignment);

                // Check for the new name
                var updatedAssignmentInDatabase = await context.Assignments.FindAsync(createdAssignment.ID);
                Assert.Equal(updatedAssignmentName, updatedAssignmentInDatabase.Name);
            }
        }

        [Fact]
        private async Task DeleteAssignmentTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAssignmentTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var assignmentsController = new AssignmentsController(context);

                var newAssignment = new Assignment{ ID = 1, Name = "Simple LMS API", DueDate = new DateTime(), Grade = "A", Module = null };
                
                context.Assignments.Add(newAssignment);
                await context.SaveChangesAsync();

                // Make sure assignment was added to context
                var createdAssignment = await context.Assignments.FindAsync(newAssignment.ID);
                Assert.Equal(newAssignment.Name, createdAssignment.Name);

                await assignmentsController.DeleteAssignment(newAssignment.ID);

                // Make sure it's gone now
                Assert.Null(await context.Assignments.FindAsync(newAssignment.ID));
            }
        }
    }
}

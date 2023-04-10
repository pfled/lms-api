using System.Net.Http;
using Xunit;
using SimpleLMS.API.Models;
using SimpleLMS.API.Controllers;
using SimpleLMS.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleLMS.API.Tests {
    public class CoursesControllerTests
    {
        [Fact]
        private async Task PostCourseTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "PostCourseTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                
                await coursesController.CreateCourse(newCourse);

                var createdCourse = await context.Courses.FindAsync(newCourse.ID);
                
                Assert.Equal(newCourse.ID, createdCourse.ID);
                Assert.Equal(newCourse.Name, createdCourse.Name);
            }
        }

        [Fact]
        private async Task GetCourseTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetCourseTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                
                context.Courses.Add(newCourse);
                await context.SaveChangesAsync();

                var result = await coursesController.GetCourse(newCourse.ID);

                var fetchedCourse = result.Value;
                
                Assert.Equal(newCourse.Name, fetchedCourse.Name);
            }
        }

        [Fact]
        private async Task GetCoursesTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "GetCoursesTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse1 = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                var newCourse2 = new Course { ID = 2, Name = "CS 380", Modules = new List<Module>() };
                
                context.Courses.Add(newCourse1);
                context.Courses.Add(newCourse2);
                await context.SaveChangesAsync();

                var result = await coursesController.GetCourses();

                var fetchedCourses = result.Value.ToList();
                
                Assert.Equal(newCourse1.Name, fetchedCourses[0].Name);
                Assert.Equal(newCourse2.Name, fetchedCourses[1].Name);
            }
        }

        [Fact]
        private async Task UpdateCourseTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "UpdateCourseTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                
                context.Courses.Add(newCourse);
                await context.SaveChangesAsync();

                // Make sure course was added with original name
                var createdCourse = await context.Courses.FindAsync(newCourse.ID);
                Assert.Equal(newCourse.Name, createdCourse.Name);

                var updatedCourseName = "CS 420: The Sequel";

                createdCourse.Name = updatedCourseName;
                await coursesController.UpdateCourse(1, createdCourse);

                // Check for the new name
                var updatedCourseInDatabase = await context.Courses.FindAsync(createdCourse.ID);
                Assert.Equal(updatedCourseName, updatedCourseInDatabase.Name);
            }
        }

        [Fact]
        private async Task DeleteCourseTest()
        {
            var options = new DbContextOptionsBuilder<SimpleLMSContext>()
                .UseInMemoryDatabase(databaseName: "DeleteCourseTest")
                .Options;
            
            using (var context = new SimpleLMSContext(options))
            {
                var coursesController = new CoursesController(context);

                var newCourse = new Course { ID = 1, Name = "CS 420", Modules = new List<Module>() };
                
                context.Courses.Add(newCourse);
                await context.SaveChangesAsync();

                // Make sure course was added to context
                var createdCourse = await context.Courses.FindAsync(newCourse.ID);
                Assert.Equal(newCourse.Name, createdCourse.Name);

                await coursesController.DeleteCourse(newCourse.ID);

                // Make sure it's gone now
                Assert.Null(await context.Courses.FindAsync(newCourse.ID));
            }
        }
    }
}

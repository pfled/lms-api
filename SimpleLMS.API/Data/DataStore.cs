using SimpleLMS.API.Models;

namespace SimpleLMS.API.Data {
    public static class DataStore
    {
        public static List<Course> Courses { get; set; } = new List<Course>();
        public static List<Module> Modules { get; set; } = new List<Module>();
        public static List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}
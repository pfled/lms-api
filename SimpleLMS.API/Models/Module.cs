namespace SimpleLMS.API.Models {
    public class Module
        {
            public int ID { get; set; }
            public string? Name { get; set; }
            public int CourseId { get; set; }
            public List<Assignment>? Assignments { get; set; }
        }
}
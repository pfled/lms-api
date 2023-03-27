namespace SimpleLMS.API.Models {
    public class Course
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public List<Module> Modules { get; set; }
    }
}
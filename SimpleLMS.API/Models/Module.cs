namespace SimpleLMS.API.Models {
    public class Module
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public Course Course { get; set; }
            public List<Assignment> Assignments { get; set; }
        }
}
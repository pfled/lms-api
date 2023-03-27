namespace SimpleLMS.API.Models {
    public class Assignment
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Grade { get; set; }
        public DateTime DueDate { get; set; }
        public int ModuleId { get; set; }
    }
}
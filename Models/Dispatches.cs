namespace OnlineBrief24.Models
{
    public class Dispatches
    {
        public Guid Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        public string Account { get; set; }
        public DateTime StartDate { get; set;}
        public virtual ICollection<File> Files { get; set; }
        public virtual Parameters Parameters { get; set; }
    }
}

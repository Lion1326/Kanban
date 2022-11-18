namespace KanbanAPI.App_Code.Models
{
    public class Issues
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreationDate { get; set; }
        public int WorkerID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int StatusID { get; set; }
        public string Description { get; set; }
    }
}

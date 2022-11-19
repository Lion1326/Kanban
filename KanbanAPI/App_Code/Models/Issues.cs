using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanAPI.App_Code.Models
{
    [Table("Issues")]
    public class Issue
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreationDate { get; set; }
        public int? WorkerID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int StatusID { get; set; }
        public string Description { get; set; }
        public User Worker { get; set; }
        public User Creator { get; set; }
        public List<TaskTime> TaskTimes { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanAPI.App_Code.Models
{

    [Table("TaskTime")]
    public class TaskTime
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public int UserID { get; set; }
        public int TimeSpent { get; set; }
        public DateTime Date  { get; set; }
    }
}

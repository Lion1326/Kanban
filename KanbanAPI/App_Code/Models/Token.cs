using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KanbanAPI.App_Code.Models
{
    [Table("UserTokens")]
    public class Token
    {
        [Key]
        public Guid TokenID { get; set; }
        public int UserID { get; set; }
        public DateTime Expires { get; set; }
    }
}

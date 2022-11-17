using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.App_Code.Models
{
    public class IssuesRepository : DbContext
    {
        public DbSet<Issues> Issues { get; set; } = null!;
    }
}

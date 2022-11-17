using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using KanbanAPI.App_Code.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

public class KanbanDBContext : DbContext
{
    public KanbanDBContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // modelBuilder.Entity<UserTokenID>()
        //     .HasKey(c => new { c.TokenID, c.UserID });
        // modelBuilder.Entity<BTHoliday>().HasKey(c => new { c.DayDate, c.CountryID });
        // modelBuilder.Entity<UsersInMailGroup>().HasKey(c => new { c.UserID, c.GroupID });

        // modelBuilder.Entity<Event>().HasMany(r => r.CreatorID).WithOne().HasForeignKey(p => p.TaskRequestID).IsRequired(false);
        //modelBuilder.Entity<Event>().HasOne(x => x.Creator).WithMany().HasForeignKey(x => x.CreatorID);
        //modelBuilder.Entity<Event>().HasMany(x => x.Fields).WithOne().HasForeignKey(x => x.EventID);
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Token> Tokens { get; set; }

}
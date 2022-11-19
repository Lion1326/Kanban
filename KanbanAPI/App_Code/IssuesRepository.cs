using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.App_Code
{
    public interface IIssueRepository
    {
        void AddPart(Issues model);
        void UppdatePart(Issues model);
        void DeletePart(Issues model);
        Task<Issues> GetIssueByID(int? id);
        Task SaveChangesAsync();
        IQueryable<Issues> GetList();
    }
    public class IssueRepository : IIssueRepository
    {
        internal KanbanDBContext _context;
        internal DbSet<Issues> _dbSet;
        public IssueRepository(KanbanDBContext context)
        {
            _context = context;
            _dbSet = context.Set<Issues>();
        }
        public void AddPart(Issues model)
        {
            _context.Entry(model).State = EntityState.Added;
        }

        public void UppdatePart(Issues model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }
        public void DeletePart(Issues model)
        {
            _context.Entry(model).State = EntityState.Deleted;
        }
        public async Task<Issues> GetIssueByID(int? id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ID == id); 
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public IQueryable<Issues> GetList()
        {
            return _dbSet;
        }
    }
}

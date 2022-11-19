using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.App_Code
{
    public interface ITaskTimeRepository
    {
        TaskTime GetByID(int id);
        IQueryable<TaskTime> GetList(int id);
        void AddPart(TaskTime model);
        void UpdatePart(TaskTime model);
        void DeletePart(TaskTime model);
        Task SaveChangesAsync();
    }
    public class TaskTimeRepository : ITaskTimeRepository
    {
        internal KanbanDBContext _context;
        internal DbSet<TaskTime> _dbSet;
        public TaskTimeRepository(KanbanDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TaskTime>();
        }
        public TaskTime GetByID(int id)
        {
            return _dbSet.SingleOrDefault(x => x.ID == id);
        }
        public IQueryable<TaskTime> GetList(int id)
        {
            return _dbSet.Where(x => x.TaskID== id);
        }
        public void AddPart(TaskTime model)
        {
            _context.Entry(model).State = EntityState.Added;
        }
        public void UpdatePart(TaskTime model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }
        public void DeletePart(TaskTime model)
        {
            _context.Entry(model).State = EntityState.Deleted;
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

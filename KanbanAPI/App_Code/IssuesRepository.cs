using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using KanbanAPI.App_Code.Models;

namespace KanbanAPI.App_Code
{
    public interface IIssueRepository
    {
        void AddPart(Issue model);
        void UppdatePart(Issue model);
        void DeletePart(Issue model);
        Task<Issue> GetIssueByID(int? id);
        Task SaveChangesAsync();
        IQueryable<Issue> GetList();
    }
    public class IssueRepository : IIssueRepository
    {
        internal KanbanDBContext _context;
        internal DbSet<Issue> _dbSet;
        public IssueRepository(KanbanDBContext context)
        {
            _context = context;
            _dbSet = context.Set<Issue>();
        }
        //Добавление Issue
        public void AddPart(Issue model)
        {
            _context.Entry(model).State = EntityState.Added;
        }

        //Редактирование Issue
        public void UppdatePart(Issue model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        //Удаление Issue
        public void DeletePart(Issue model)
        {
            _context.Entry(model).State = EntityState.Deleted;
        }

        //Получние Isuue по ID
        public async Task<Issue> GetIssueByID(int? id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ID == id); 
        }

        //Сохранение асинхронного метода
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //Получение списка Issue
        public IQueryable<Issue> GetList()
        {
            return _dbSet;
        }
    }
}
